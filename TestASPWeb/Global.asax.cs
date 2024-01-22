using DataLibrary.BusinessLogic.Logger;
using DataLibrary.BusinessLogic.Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace TestASPWeb
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private ILogger _logger;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //quartz
            //JobScheduler.Start(); 
        }

        protected void Application_BeginRequest()
        {
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.Cache.SetNoStore();
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            Exception ex = Server.GetLastError();

            _logger = new Logger("Log.txt");
            _logger.LogError(ex);


            if (ex is HttpException httpException)
            {
                Response.StatusCode = httpException.GetHttpCode();

                if (Response.StatusCode == 404)
                {
                    Response.Redirect("/Error/Error404");
                }
                else
                {
                    Response.Redirect("/Error/Error500");
                }
            }
            else
            {
                Response.Redirect("/Error/InternalError");
            }

        }
    }
}
