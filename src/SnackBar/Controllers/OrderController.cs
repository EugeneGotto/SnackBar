using AutoMapper;
using SnackBar.BLL.Interfaces;
using SnackBar.DAL.Models;
using SnackBar.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace SnackBar.Controllers
{
    public class OrderController : BaseController
    {
        public OrderController(IServiceFactory factory) : base(factory)
        {
        }

        [HttpGet]
        public ActionResult AddOrder(int? id)
        {
            if (!id.HasValue)
            {
                return RedirectToAction("Index", "Home", new { area = string.Empty });
            }

            var product = Factory.GetProductService().GetProductById((int)id);
            if (product == null)
            {
                return HttpNotFound();
            }

            var model = new ProductListViewModel()
            {
                Pin = HttpContext.Request.Cookies["PIN"]?.Value,
                Products = new List<ProductViewModel>()
                {
                    Mapper.Map<ProductViewModel>(product),
                }
            };

            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddOrder(OrderViewModel model)
        {
            var orderService = Factory.GetOrderService();
            var order = Mapper.Map<Order>(model);
            order = orderService.AddNewOrder(order);
            if (order == null)
            {
                return HttpNotFound();
            }

            var cookie = new HttpCookie("PIN")
            {
                Value = order.Pin,
                Expires = DateTime.Now.AddMonths(1),
            };

            Response.SetCookie(cookie);

            //Send message to trigger
            HttpClient client = new HttpClient();
            var uri = new Uri("https://maker.ifttt.com/trigger/snackbar_buy_product/with/key/bH0X-dChSor4CXMIstTPk_");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var message = new OrderForMakerIFTTTViewModel()
            {
                Company = ConfigurationManager.AppSettings["CompanyName"],
                Date = order.Date.ToString("G"),
                Email = order.User.Email,
                Price = string.Format("{0} BYN", order.Price),
                ProductName = order.Product.Name
            };

            HttpResponseMessage response = await client.PostAsJsonAsync(uri, message);
            //End sending message

            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }
    }
}