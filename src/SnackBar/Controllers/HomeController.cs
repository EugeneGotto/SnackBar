using AutoMapper;
using NLog;
using SnackBar.BLL.Interfaces;
using SnackBar.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SnackBar.Controllers
{
    public class HomeController : BaseController
    {
        private Logger _logger = LogManager.GetLogger("Users");

        public HomeController(IServiceFactory factory) : base(factory)
        {
        }

        [HttpGet]
        public ViewResult Index()
        {
            var products = Factory.GetProductService().GetAllProducts(p => p.Count > 0);
            var model = new ProductListViewModel()
            {
                Pin = HttpContext.Request.Cookies["PIN"]?.Value,
                Products = Mapper.Map<List<ProductViewModel>>(products),
            };

            if (!string.IsNullOrEmpty(model.Pin))
            {
                var userService = Factory.GetUserService();
                model.UserInfo = userService.GetIndexPageInfo(model.Pin);
            }

            return View(model);
        }

        [HttpGet]
        public ActionResult TagList(long id = 0)
        {
            var tags = Factory.GetTagService().GetAllTags();
            ViewBag.SelectedTag = id;

            return PartialView("_tagList", tags);
        }

        [HttpGet]
        public ActionResult Category(long? id)
        {
            if (!id.HasValue || id.Value <= 0)
            {
                return this.HttpNotFound();
            }

            var products = Factory.GetTagService().GetProductsByTagId(id.Value);

            if (products == null)
            {
                return this.HttpNotFound();
            }

            var model = new ProductListViewModel()
            {
                Pin = HttpContext.Request.Cookies["PIN"]?.Value,
                Products = Mapper.Map<List<ProductViewModel>>(products),
            };

            if (!string.IsNullOrEmpty(model.Pin))
            {
                var userService = Factory.GetUserService();
                model.UserInfo = userService.GetIndexPageInfo(model.Pin);
            }

            this.TempData["SelectedTag"] = id;
            return View("Index", model);
        }

        [HttpPost]
        public async Task<ActionResult> CreatePin(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                this.TempData["ErrorEmailMessage"] = Resx.Resource.ErrorEmailEmpty;
                return this.RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            _logger.Info($"Start Looking user with Email - \"{email}\"...");
            var address = new MailAddress(email);
            string emailDomen = ConfigurationManager.AppSettings["EmailDomen"];
            if (!emailDomen.Equals(address.Host.ToLower()))
            {
                _logger.Info("Incorrect Email");
                TempData["ErrorEmailMessage"] = string.Format(Resx.Resource.ErrorEmailDomen, ConfigurationManager.AppSettings["CompanyName"]);
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            var userService = this.Factory.GetUserService();

            var user = await userService.RegisterUser(email);

            if (user == null)
            {
                this._logger.Warn($"Error with creating User with email - \"{email}\"");
                TempData["ErrorEmailMessage"] = Resx.Resource.ErrorCreatingUser;
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            _logger.Info($"Success");
            TempData["SuccessMessage"] = Resx.Resource.SuccessMessage;
            return RedirectToAction("Index");
        }

        public ActionResult ChangeCulture(string lang)
        {
            string returnUrl = Request.UrlReferrer.AbsolutePath;
            List<string> cultures = new List<string>() { "en", "ru" };
            if (!cultures.Contains(lang))
            {
                lang = "en";
            }

            HttpCookie cookie = Request.Cookies["lang"];
            if (cookie != null)
            {
                cookie.Value = lang;
            }
            else
            {
                cookie = new HttpCookie("lang");
                cookie.HttpOnly = false;
                cookie.Value = lang;
                cookie.Expires = DateTime.Now.AddYears(1);
            }

            Response.Cookies.Add(cookie);
            return Redirect(returnUrl);
        }
    }
}