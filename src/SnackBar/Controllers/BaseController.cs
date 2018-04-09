using SnackBar.BLL.Interfaces;
using SnackBar.Filters;
using System.Web.Mvc;

namespace SnackBar.Controllers
{
    [Culture]
    public class BaseController : Controller
    {
        protected readonly IServiceFactory Factory;

        public BaseController(IServiceFactory factory)
        {
            this.Factory = factory;
        }
    }
}