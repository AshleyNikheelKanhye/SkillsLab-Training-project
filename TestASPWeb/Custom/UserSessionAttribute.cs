using System.Web.Mvc;

namespace TestASPWeb.Custom
{
    //add on top of every contoller to see if the user has logged in before accessing
    public class UserSessionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (filterContext.HttpContext.Session["CurrentUser"] ==null || filterContext.HttpContext.Session["CurrentRole"] == null)
            {
                filterContext.Result = new RedirectResult("~/User/Login");
            }
        }


    }
}