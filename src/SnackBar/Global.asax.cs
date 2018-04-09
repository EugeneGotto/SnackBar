using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace SnackBar
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static NLog.Logger _logger = NLog.LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            _logger.Trace("Application Start");

            GlobalConfiguration.Configure(WebApiConfig.Register);
            AreaRegistration.RegisterAllAreas();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        public void Init()
        {
            _logger.Trace("Application Init");
        }

        public void Dispose()
        {
            _logger.Trace("Application Dispose");
        }

        protected void Application_Error()
        {
            _logger.Trace("Application Error");
        }

        protected void Application_End()
        {
            _logger.Trace("Application End");
        }
    }
}