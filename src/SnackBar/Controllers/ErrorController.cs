using SnackBar.BLL.Interfaces;
using System.Web.Mvc;

namespace SnackBar.Controllers
{
    public class ErrorController : BaseController
    {
        public ErrorController(IServiceFactory factory) : base(factory)
        {
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Unauthorized()
        {
            return View();
        }

        public ActionResult Blocked()
        {
            return View();
        }

        public ActionResult Unconfirmed()
        {
            return View();
        }

        public ActionResult RegisterError()
        {
            return View();
        }

        public ActionResult InvalidRequestParameter()
        {
            return View();
        }

        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult InnerException()
        {
            return View();
        }
    }
}