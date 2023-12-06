using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestASPWeb.Controllers
{
    public class EmployeeController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetUserDetails()
        {
            var userDetails = this.Session["CurrentUser"];
            return Json(new { employee = this.Session["CurrentUser"] },JsonRequestBehavior.AllowGet);
        }
    }
}