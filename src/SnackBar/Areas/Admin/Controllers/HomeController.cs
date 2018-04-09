using AutoMapper;
using Microsoft.AspNet.Identity;
using NLog;
using SnackBar.BLL.Interfaces;
using SnackBar.Controllers;
using SnackBar.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SnackBar.Areas.Admin.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private Logger _logger = LogManager.GetLogger("AdminEmail");

        public HomeController(IServiceFactory factory) : base(factory)
        {
        }

        // GET: /Admin/Home/Index
        public ActionResult Index()
        {
            var debitors = Factory.GetAdminService().GetAllDebitors();
            var model = Mapper.Map<IEnumerable<UserViewModel>>(debitors);

            return View(model);
        }

        [HttpGet]
        public async Task<ActionResult> SendAsync()
        {
            _logger.Info($"{User.Identity.Name} start sending Emails...");
            var users = Factory.GetAdminService().GetAllDebitors();
            var debitors = Mapper.Map<IEnumerable<UserViewModel>>(users);
            if (debitors != null)
            {
                try
                {
                    foreach (var user in debitors)
                    {
                        var message = new IdentityMessage()
                        {
                            Subject = "From SnackBar",
                            Body = $"Your Balance is {user.Balance}, please pay!",
                            Destination = user.Email
                        };
                        var client = new SnackBar.EmailService();
                        await client.SendAsync(message).ConfigureAwait(false);
                    }
                }
                catch
                {
                    _logger.Warn("Email not sends");
                    TempData["ErrorEmailSends"] = Resx.Resource.ErrorEmailsNotSent;
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }

            _logger.Info($"{User.Identity.Name} send all emails.");
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }
    }
}