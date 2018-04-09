using SnackBar.BLL.Interfaces;
using System.Web.Http;

namespace SnackBar.Areas.API.Controllers
{
    [Authorize]
    public class PaymentsController : BaseApiController
    {
        public PaymentsController(IServiceFactory factory) : base(factory)
        {
        }
    }
}