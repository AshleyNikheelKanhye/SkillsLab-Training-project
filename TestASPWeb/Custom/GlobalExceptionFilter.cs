using DataLibrary.BusinessLogic.Logger;
using System.Web.Mvc;

namespace TestASPWeb.Custom
{
    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger Logger;
        public GlobalExceptionFilter(ILogger _logger)
        {
            this.Logger = _logger;
        }
        public void OnException(ExceptionContext filterContext)
        {
            Logger.LogError(filterContext.Exception);

            filterContext.ExceptionHandled = true;
            filterContext.Result = new ViewResult()
            {
                ViewName = "Error",
                TempData = filterContext.Controller.TempData
            };


        }
    }
}