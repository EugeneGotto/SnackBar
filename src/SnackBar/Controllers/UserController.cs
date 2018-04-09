using AutoMapper;
using NLog;
using SnackBar.BLL.Interfaces;
using SnackBar.ViewModels;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;

namespace SnackBar.Controllers
{
    public class UserController : BaseController
    {
        private Logger _logger = LogManager.GetLogger("Users");

        public UserController(IServiceFactory factory) : base(factory)
        {
        }

        [HttpGet]
        public ActionResult Balance(string pin)
        {
            if (!string.IsNullOrEmpty(pin))
            {
                var userService = Factory.GetUserService();
                var returnResult = userService.GetIndexPageInfo(pin);
                var cookie = new HttpCookie("PIN")
                {
                    Value = pin,
                    Expires = DateTime.Now.AddMonths(1),
                };

                Response.SetCookie(cookie);

                return PartialView(returnResult);
            }

            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }

        [HttpPost]
        public ActionResult Login(string Pin)
        {
            if (string.IsNullOrEmpty(Pin))
            {
                TempData["ErrorPinMessage"] = Resx.Resource.ErrorPinEmpty;
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            if (Pin.Length != 4)
            {
                TempData["ErrorPinMessage"] = Resx.Resource.ErrorPinLenght;
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            var user = Factory.GetUserService().GetUserEmailByPin(Pin);
            if (user == null)
            {
                ViewBag.ErrorMessage = $"User not found";
                _logger.Info("Incorrect Email");
                TempData["ErrorPinMessageRed"] = Resx.Resource.User404;
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            var cookie = new HttpCookie("PIN")
            {
                Value = Pin,
                Expires = DateTime.Now.AddDays(14),
            };

            Response.SetCookie(cookie);

            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }

        [HttpGet]
        public ActionResult LogOut()
        {
            if (Request.Cookies["PIN"] != null)
            {
                var c = new HttpCookie("PIN");
                c.Expires = DateTime.Now.AddDays(-1);
                Response.Cookies.Add(c);
                Session.Abandon();
            }

            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }

        [HttpGet]
        public ActionResult GetOrders()
        {
            var pin = HttpContext.Request.Cookies["PIN"]?.Value;
            if (string.IsNullOrEmpty(pin))
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            var orders = Factory.GetOrderService().GetLastUserOrders(pin);
            var model = Mapper.Map<IEnumerable<OrderListViewModel>>(orders);

            return this.PartialView(model);
        }
    }
}