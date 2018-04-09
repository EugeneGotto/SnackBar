using SnackBar.BLL.Interfaces;
using System.Web.Http;

namespace SnackBar.Areas.API.Controllers
{
    public class BaseApiController : ApiController
    {
        protected readonly IServiceFactory Factory;

        public BaseApiController(IServiceFactory factory)
        {
            Factory = factory;
        }
    }
}