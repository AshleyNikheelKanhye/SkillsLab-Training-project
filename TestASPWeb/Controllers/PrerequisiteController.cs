using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestASPWeb.Controllers
{
    public class PrerequisiteController : Controller
    {
        private readonly IPrerequisiteService _prerequisiteService;

        public PrerequisiteController(IPrerequisiteService prerequisiteService)
        {
            this._prerequisiteService = prerequisiteService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPrerequisitesNotInEmployee()
        {
            var listPrerequisites = _prerequisiteService.GetPrerequisitesNotInEmployee((int)this.Session["CurrentUserID"]);
            if(listPrerequisites != null)
            {
                return Json(new { listQualifications =listPrerequisites},JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new {error = "error" });
            }
        }



        //admin when inserting a new training
        [HttpGet]
        public JsonResult GetAllPrerequisites()
        {
            var list = _prerequisiteService.GetAllPrerequisites();
            if(list != null)
            {
                return Json(new {listPrerequisites=list},JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new {error = "error"}, JsonRequestBehavior.AllowGet); 
            }
        }

        [HttpPost]
        public JsonResult GetUserPrerequisiteForEnrollment(int enrollmentID)
        {
            var listQualifications = _prerequisiteService.GetUserPrerequisiteForEnrollment(enrollmentID);
            if( listQualifications != null)
            {
                return Json(new { list=listQualifications},JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = "error" },JsonRequestBehavior.AllowGet);   
            }
        }

        public ActionResult DownloadQualification(int userID,int prerequisiteID)
        {
            EmployeeQualification qualification = _prerequisiteService.DownloadQualification(userID, prerequisiteID);
            if (qualification != null)
            {
                return File(qualification.FileContent, "application/pdf", qualification.FileName);
            }
            else
            {
                return Content("file not found");
            }
        }

        
    }
}