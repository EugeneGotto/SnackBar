using NLog;
using SnackBar.BLL.Interfaces;
using SnackBar.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace SnackBar.Controllers
{
    public class CartController : BaseController
    {
        private Logger _logger = LogManager.GetLogger("Cart");

        public CartController(IServiceFactory factory) : base(factory)
        {
        }

        // GET: Cart
        public ActionResult Index()
        {
            var pin = HttpContext.Request.Cookies["PIN"]?.Value;
            CartViewModel model = null;
            if (string.IsNullOrEmpty(pin))
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            var userService = Factory.GetUserService();
            var balance = userService.GetBalanceByPin(pin);
            var email = userService.GetUserEmailByPin(pin);
            var cart = Factory.GetCartService().GetCartByPin(pin);

            if (email == null || balance == null || cart == null)
            {
                return View(model);
            }

            model = new CartViewModel()
            {
                Balance = (decimal)balance,
                BuyerName = email.User,
                Cart = cart
            };

            return View(model);
        }

        [HttpGet]
        [Route("Cart/AddItem/{Id}")]
        public ActionResult AddItem(long Id)
        {
            var pin = HttpContext.Request.Cookies["PIN"]?.Value;
            if (string.IsNullOrEmpty(pin))
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            var cart = Factory.GetCartService().AddItem(pin, Id);
            if (cart == null)
            {
                _logger.Info($"Error while adding to cart. View Errors log");
                //TODO: Handle of errors
            }

            Session["IsModifiedOrdersCount"] = true;
            return RedirectToAction("Index", "Home", new { area = string.Empty });
        }

        [HttpGet]
        [Route("Cart/IncreaseCount/{Id}")]
        public ActionResult IncreaseCount(long Id)
        {
            var pin = HttpContext.Request.Cookies["PIN"]?.Value;
            if (string.IsNullOrEmpty(pin))
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            var cart = Factory.GetCartService().InDeCreaseQuantity(Id, 1);
            Session["IsModifiedOrdersCount"] = true;
            return PartialView("_cartTable", cart);
        }

        [HttpGet]
        [Route("Cart/DecreaseCount/{Id}")]
        public ActionResult DecreaseCount(long Id)
        {
            var pin = HttpContext.Request.Cookies["PIN"]?.Value;
            if (string.IsNullOrEmpty(pin))
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            var cart = Factory.GetCartService().InDeCreaseQuantity(Id, -1);
            Session["IsModifiedOrdersCount"] = true;
            return PartialView("_cartTable", cart);
        }

        [HttpGet]
        [Route("Cart/RemoveItem/{Id}")]
        public ActionResult RemoveItem(long Id)
        {
            var pin = HttpContext.Request.Cookies["PIN"]?.Value;
            if (string.IsNullOrEmpty(pin))
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            var cart = Factory.GetCartService().RemoveItem(Id);
            Session["IsModifiedOrdersCount"] = true;
            return PartialView("_cartTable", cart);
        }

        [HttpGet]
        [Route("Cart/MakeOrder")]
        public ActionResult MakeOrder()
        {
            var pin = HttpContext.Request.Cookies["PIN"]?.Value;
            pin = pin.ToUpper();
            if (string.IsNullOrEmpty(pin))
            {
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            _logger.Info("Start making order");
            var result = Factory.GetCartService().MakeOrder(pin);
            if (result == null)
            {
                _logger.Info("Making order Error. View Errors Log");
                return RedirectToAction("Login", "Account", new { area = "Admin" });
            }

            if (result.CheckLines.Any())
            {
                _logger.Info($"Order successfull");
                Session["IsModifiedOrdersCount"] = true;
                return View("Check", result);
            }

            _logger.Info($"Cart was Empty");
            return RedirectToAction("Index");
        }

        public int Summary()
        {
            if (Session["IsModifiedOrdersCount"] == null)
            {
                Session["IsModifiedOrdersCount"] = true;
            }

            int count = 0;
            if (!(bool)Session["IsModifiedOrdersCount"])
            {
                count = (int)Session["OrdersCount"];
                return count;
            }

            var pin = HttpContext.Request.Cookies["PIN"]?.Value;

            if (string.IsNullOrEmpty(pin))
            {
                return 0;
            }

            var cart = Factory.GetCartService().GetCartByPin(pin);
            if (cart != null)
            {
                count = cart.TotalItems;
                if (Session != null)
                {
                    Session["OrdersCount"] = count;
                    Session["IsModifiedOrdersCount"] = false;
                }
            }

            return count;
        }
    }
}