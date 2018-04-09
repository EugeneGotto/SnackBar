using AutoMapper;
using SnackBar.BLL.Interfaces;
using SnackBar.DAL.Models;
using SnackBar.ViewModels;
using Swashbuckle.Swagger.Annotations;
using System.Web.Http;

namespace SnackBar.Areas.API.Controllers
{
    /// <summary>
    /// API Order
    /// </summary>
    public class OrdersController : BaseApiController
    {
        public OrdersController(IServiceFactory factory) : base(factory)
        {
        }

        // POST: api/Orders
        /// <summary>
        /// Add new Order
        /// </summary>
        /// <param name="model">Added Order</param>
        [AllowAnonymous]
        [Route("Api/Orders")]
        [SwaggerResponse(200, Description = "Add new Order", Type = (typeof(OrderApiViewModel)))]
        [SwaggerResponse(400, Description = "Bad model of new Order")]
        public IHttpActionResult PostOrder([FromBody]OrderViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var order = Mapper.Map<Order>(model);
            order = Factory.GetOrderService().AddNewOrder(order);
            if (order == null)
            {
                return BadRequest();
            }

            var returnResult = Mapper.Map<OrderApiViewModel>(order);
            return Ok(returnResult);
        }
    }
}