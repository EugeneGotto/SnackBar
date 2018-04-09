using System.Web.Http;
using System.Web.Mvc;

namespace SnackBar.Areas.API
{
    public class APIAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "API";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.Routes.MapHttpRoute(
                "API_WebApiRoute",
                "Api/{controller}/{id}",
                new { id = RouteParameter.Optional }
            );
        }
    }
}