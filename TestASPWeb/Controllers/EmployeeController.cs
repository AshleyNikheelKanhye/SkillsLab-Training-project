using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities;
using DataLibrary.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestASPWeb.Custom;

namespace TestASPWeb.Controllers
{
    [UserSession]
    [CustomAuthorization("Employee")]
    public class EmployeeController : Controller
    {
        private readonly IPrerequisiteService _prerequisiteService;
        private readonly IUserNotificationService _userNotificationService;

        public EmployeeController(IPrerequisiteService prerequisiteService,IUserNotificationService userNotificationService)
        {
            this._prerequisiteService = prerequisiteService;
            this._userNotificationService = userNotificationService;
        }

        [HttpGet]
        public JsonResult GetUserDetails()
        {
            return Json(new { currentUser = this.Session["CurrentUser"] },JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult GetEmployeeQualifications() 
        { 
            var list = _prerequisiteService.GetEmployeeQualifications((int)this.Session["CurrentUserID"]);
            if(list != null)
            {
                return Json(new { result = list }, JsonRequestBehavior.AllowGet);    
            }
            else
            {
                return Json(new {error="an error occured"},JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult DownloadQualification(int prerequisiteID)
        {
            EmployeeQualification qualification = _prerequisiteService.DownloadQualification((int)this.Session["CurrentUserID"], prerequisiteID);
            if(qualification != null)
            {
                return File(qualification.FileContent, "application/pdf", qualification.FileName);
            }
            else
            {
                return Content("file not found");
            }
        }

        [HttpPost]
        public ActionResult UploadQualifications(HttpPostedFileBase file ,int prerequisiteID)
        {
            try
            {
                if(file != null && file.ContentLength > 0)
                {
                    int userID = (int)this.Session["CurrentUserID"];
                    string fileName = file.FileName;
                    bool uploadResult = _prerequisiteService.UploadQualifications(file, prerequisiteID,userID,fileName);
                    if (uploadResult)
                    {
                        return Json(new { result = true }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new {result=false},JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = false}, JsonRequestBehavior.AllowGet);
                }
            }catch
            {
                return Json(new {result =false}, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult EmployeeView()
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

