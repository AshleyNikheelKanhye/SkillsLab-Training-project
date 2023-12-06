using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestASPWeb.Custom;

namespace TestASPWeb.Controllers
{
    [UserSessionAttribute]
    [CustomAuthorizationAttribute("employee")]//authorization
    public class EmployeeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        
        public JsonResult GetUserDetails()
        {
            var userDetails = this.Session["CurrentUser"]; //for debuging purposes
            return Json(new { currentUser = this.Session["CurrentUser"] },JsonRequestBehavior.AllowGet);
        }
    }
}

