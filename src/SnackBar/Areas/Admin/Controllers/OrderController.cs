using AutoMapper;
using SnackBar.BLL.Interfaces;
using SnackBar.Controllers;
using SnackBar.ViewModels;
using System.Collections.Generic;
using System.Net.Mail;
using System.Web.Mvc;

namespace SnackBar.Areas.Admin.Controllers
{
    [Authorize]
    public class OrderController : BaseController
    {
        public OrderController(IServiceFactory factory) : base(factory)
        {
        }

        [HttpGet]
        public ActionResult GetTodayOrders()
        {
            var orders = Factory.GetOrderService().GetAllTodayOrders();
            var model = Mapper.Map<IEnumerable<OrderListViewModel>>(orders);

            return View(model);
        }

        [HttpGet]
        public ActionResult GetMonthOrders()
        {
            var orders = Factory.GetOrderService().GetAllMonthOrders();
            var model = Mapper.Map<IEnumerable<OrderListViewModel>>(orders);

            return View(model);
        }

        [HttpGet]
        public ActionResult GetLastMOrders()
        {
            var orders = Factory.GetOrderService().GetAllLastMonthOrders();
            var model = Mapper.Map<IEnumerable<OrderListViewModel>>(orders);

            return View(model);
        }

        [HttpGet]
        public ActionResult GetAllOrders()
        {
            var orders = Factory.GetOrderService().GetAllOrders();
            var model = Mapper.Map<IEnumerable<OrderListViewModel>>(orders);

            return View(model);
        }

        [HttpGet]
        public ActionResult GetUserOrders(string input)
        {
            MailAddress email = null;
            if (input.Length == 4)
            {
                email = Factory.GetUserService().GetUserEmailByPin(input);
            }
            else
            {
                try
                {
                    email = new MailAddress(input);
                }
                catch
                {
                    TempData["ErrorEmailInput"] = Resx.Resource.ErrorEmailInput;
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
            }

            if (email == null)
            {
                TempData["ErrorEmailInput"] = Resx.Resource.ErrorEmailInput;
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            var orders = Factory.GetOrderService().GetAllUserOrders(email.Address);

            if (orders == null)
            {
                TempData["ErrorEmailInput"] = Resx.Resource.User404;
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            }

            var model = Mapper.Map<IEnumerable<OrderListViewModel>>(orders);

            ViewBag.UserName = email.User;
            return View(model);
        }
    }
}