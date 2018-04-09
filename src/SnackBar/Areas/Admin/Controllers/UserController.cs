using AutoMapper;
using NLog;
using SnackBar.BLL.Interfaces;
using SnackBar.Controllers;
using SnackBar.ViewModels;
using System.Collections.Generic;
using System.Globalization;
using System.Web.Mvc;

namespace SnackBar.Areas.Admin.Controllers
{
    [Authorize]
    public class UserController : BaseController
    {
        private Logger _logger = LogManager.GetLogger("AdminPayments");

        public UserController(IServiceFactory factory) : base(factory)
        {
        }

        [HttpGet]
        public ActionResult AddPayment()
        {
            return View();
        }

        [HttpGet]
        public ActionResult AddPaymentByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return View("AddPayment");
            }

            var balance = Factory.GetUserService().GetBalanceByEmail(email);

            if (balance == null)
            {
                return View("AddPayment");
            }

            var model = new PaymentViewModel()
            {
                Email = email,
                Money = (balance * -1).ToString()
            };

            return View(model);
        }

        [HttpPost]
        public ActionResult AddPayment(PaymentViewModel model)
        {
            var separator = CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator;

            model.Money = model.Money.Replace(",", separator).Replace(".", separator).Trim();
            decimal money = 0M;
            if (!decimal.TryParse(model.Money, out money))
            {
                return HttpNotFound();
            }

            var payService = Factory.GetPaymentService();
            if (model == null || payService.AddNewPayment(model.Email, money) == null)
            {
                return HttpNotFound();
            }

            _logger.Info($"{User.Identity.Name} Changed balance of \"{model.Email}\" for \'{model.Money} BYN\'");
            return RedirectToAction("Index", "Home", new { area = "Admin" });
        }

        [HttpGet]
        public ActionResult GetUserPayments(string email)
        {
            var payments = Factory.GetPaymentService().GetAllUserPayments(email);
            var model = Mapper.Map<IEnumerable<PaymentListViewModel>>(payments);

            return View("GetLastPayments", model);
        }

        [HttpGet]
        public ActionResult GetLastPayments()
        {
            var payments = Factory.GetPaymentService().GetLastPayments();
            var model = Mapper.Map<IEnumerable<PaymentListViewModel>>(payments);

            return View(model);
        }
    }
}