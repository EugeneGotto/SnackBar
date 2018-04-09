using AutoMapper;
using SnackBar.BLL.Interfaces;
using SnackBar.ViewModels;
using Swashbuckle.Swagger.Annotations;
using System.Web.Http;

namespace SnackBar.Areas.API.Controllers
{
    [RoutePrefix("Api/Carts")]
    public class CartsController : BaseApiController
    {
        public CartsController(IServiceFactory factory) : base(factory)
        {
        }

        /// <summary>
        /// Get all items in cart
        /// </summary>
        /// <param name="pin">User PIN</param>
        [Route("{pin}")]
        [HttpGet]
        [SwaggerResponse(200, Description = "Byers Cart", Type = (typeof(CartApiViewModel)))]
        [SwaggerResponse(400, Description = "Incorrect PIN")]
        [SwaggerResponse(404, Description = "User not found")]
        public IHttpActionResult GetCartByPin(string pin)
        {
            if (string.IsNullOrEmpty(pin) || pin.Length != 4)
            {
                return BadRequest("Incorrect PIN");
            }

            var cart = Factory.GetCartService().GetCartByPin(pin);
            if (cart == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<CartApiViewModel>(cart);
            return Ok(result);
        }

        /// <summary>
        /// Add Item to Cart
        /// </summary>
        /// <param name="pin">User PIN</param>
        /// <param name="productId">Added product ID</param>
        [HttpPost]
        [Route("{pin}/{productId}")]
        [SwaggerResponse(200, Description = "Add Successfully", Type = (typeof(CartApiViewModel)))]
        [SwaggerResponse(400, Description = "Incorrect PIN")]
        [SwaggerResponse(404, Description = "User not found")]
        public IHttpActionResult AddItem(string pin, long productId)
        {
            if (string.IsNullOrEmpty(pin) || pin.Length != 4)
            {
                return BadRequest("Incorrect PIN");
            }

            var cart = Factory.GetCartService().AddItem(pin, productId);
            if (cart == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<CartApiViewModel>(cart);
            return Ok(result);
        }

        /// <summary>
        /// Make order for user by PIN
        /// </summary>
        /// <param name="pin">User PIN</param>
        [HttpPost]
        [Route("{pin}/MakeOrder")]
        [SwaggerResponse(200, Description = "Order created", Type = (typeof(CheckViewModel)))]
        [SwaggerResponse(400, Description = "Incorrect PIN")]
        [SwaggerResponse(404, Description = "User not found")]
        public IHttpActionResult MakeOrder(string pin)
        {
            if (string.IsNullOrEmpty(pin) || pin.Length != 4)
            {
                return BadRequest("Incorrect PIN");
            }

            var cart = Factory.GetCartService().MakeOrder(pin);
            if (cart == null)
            {
                return NotFound();
            }

            return Ok(cart);
        }

        /// <summary>
        /// Change ordered count
        /// </summary>
        /// <param name="pin">User PIN</param>
        /// <param name="cartItemId">Cart Item ID</param>
        /// <param name="count">New Ordered Count</param>
        [HttpPost]
        [Route("{pin}/ChangeCount/{cartItemId}/{count:int}")]
        [SwaggerResponse(200, Description = "Updated Successfully", Type = (typeof(CartApiViewModel)))]
        [SwaggerResponse(400, Description = "Incorrect PIN or Count")]
        [SwaggerResponse(404, Description = "User not found")]
        public IHttpActionResult ChangeCount(string pin, long cartItemId, int count)
        {
            if (string.IsNullOrEmpty(pin) || pin.Length != 4 || cartItemId <= 0)
            {
                return BadRequest("Incorrect PIN");
            }

            if (count <= 0)
            {
                return BadRequest("Count is Incorrect");
            }

            var cart = Factory.GetCartService().ChangeQuantity(cartItemId, count);
            if (cart == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<CartApiViewModel>(cart);
            return Ok(result);
        }

        /// <summary>
        /// Remove product from Cart
        /// </summary>
        /// <param name="pin">User PIN</param>
        /// <param name="cartItemId">Cart Item ID</param>
        [HttpDelete]
        [Route("{pin}/Delete/{cartItemId}")]
        [SwaggerResponse(200, Description = "Deleted successully", Type = (typeof(CartApiViewModel)))]
        [SwaggerResponse(400, Description = "Incorrect PIN")]
        [SwaggerResponse(404, Description = "User not found")]
        public IHttpActionResult DeleteCartItem(string pin, long cartItemId)
        {
            if (string.IsNullOrEmpty(pin) || pin.Length != 4 || cartItemId <= 0)
            {
                return BadRequest("Incorrect PIN");
            }

            var cart = Factory.GetCartService().RemoveItem(cartItemId);
            if (cart == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<CartApiViewModel>(cart);
            return Ok(result);
        }

        /// <summary>
        /// Clear Cart
        /// </summary>
        /// <param name="pin">User PIN</param>
        [HttpPost]
        [Route("{pin}/Clear")]
        [SwaggerResponse(200, Description = "Clear successful")]
        [SwaggerResponse(400, Description = "Incorrect PIN")]
        [SwaggerResponse(404, Description = "User not found")]
        public IHttpActionResult ClearCart(string pin)
        {
            if (string.IsNullOrEmpty(pin) || pin.Length != 4)
            {
                return BadRequest("Incorrect PIN");
            }

            var cart = Factory.GetCartService().Clear(pin);
            if (cart == null)
            {
                return NotFound();
            }

            var result = Mapper.Map<CartApiViewModel>(cart);
            return Ok();
        }
    }
}