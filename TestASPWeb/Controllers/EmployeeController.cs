using DataLibrary.BusinessLogic.BusinessLogicInterface;
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
        private readonly IPrerequisiteService _prerequisiteService;


        public EmployeeController(IPrerequisiteService prerequisiteService)
        {
            this._prerequisiteService = prerequisiteService;
        }


        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public JsonResult GetUserDetails()
        {
            return Json(new { currentUser = this.Session["CurrentUser"] },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmployeeQualifications(int userID) 
        { 
            var list = _prerequisiteService.GetEmployeeQualifications(userID);
            return Json(new { result = list }, JsonRequestBehavior.AllowGet);    
        }



        public ActionResult GetHomeView()
        {
            return PartialView("~/Views/Employee/_homePartialView.cshtml");
        }
        public ActionResult GetProfileView()
        {
            return PartialView("~/Views/Employee/_profilePartialView.cshtml");
        }




        public ActionResult EmployeeView()
        {
            return View();
        }

        public ActionResult GetProfile()
        {
            return View();
        }
        public ActionResult GetTrainingView()
        {
            return View();
        }

        public ActionResult QualificationsView()
        {
            return View();
        }



    }
}

