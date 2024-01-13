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
    [UserSessionAttribute]
    [CustomAuthorizationAttribute("Employee")]//authorization
    public class EmployeeController : Controller
    {
        private readonly IPrerequisiteService _prerequisiteService;
        private readonly IUserNotificationService _userNotificationService;



        public EmployeeController(IPrerequisiteService prerequisiteService,IUserNotificationService userNotificationService)
        {
            this._prerequisiteService = prerequisiteService;
            this._userNotificationService = userNotificationService;
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

        public ActionResult GetHomeView()
        {
            return PartialView("~/Views/Employee/_homePartialView.cshtml");
        }
        public ActionResult GetProfileView()
        {
            return PartialView("~/Views/Employee/_profilePartialView.cshtml");
        }

        [HttpPost]
        public ActionResult UploadQualifications(HttpPostedFileBase file ,int prerequisiteID)
        {
            try
            {
                if(file != null && file.ContentLength > 0)
                {
                    //add constraints to check if extension is pdf : https://www.compilemode.com/2017/02/uploading-downloading-pdf-files-from-database-in-asp-net-mvc.html
                    int userID = (int)this.Session["CurrentUserID"];
                    string fileName = file.FileName;
                    
                    bool uploadResult = _prerequisiteService.UploadQualifications(file, prerequisiteID,userID,fileName);
                    if (uploadResult)
                    {
                        return Json(new { result = "got file in backend" }, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        return Json(new {result="could not upload this file"},JsonRequestBehavior.AllowGet);
                    }
                }
                else
                {
                    return Json(new { result = " got the file but it is null" }, JsonRequestBehavior.AllowGet);
                }
            }catch(Exception ex)
            {
                return Json(new {result ="error could not get pdf in backend"}, JsonRequestBehavior.AllowGet);
            }
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

        public ActionResult InboxView()
        {
            return View();
        }


    }
}

