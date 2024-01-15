using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.BusinessLogic.Logger;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using TestASPWeb.Custom;

namespace TestASPWeb.Controllers
{
    [UserSession]
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            this._enrollmentService = enrollmentService;
        }

        [CustomAuthorization("Manager")]
        [HttpPost]
        public async Task<JsonResult> ManagerUpdatesEnrollment(int enrollmentID,string ManagerResult,string DisapproveMessage)
        {
            bool status = _enrollmentService.ManagerUpdatesEnrollment(enrollmentID, ManagerResult);
            if (status)
            {
                await _enrollmentService.ManagerSendMailToEmployee(enrollmentID,GetUserID(),DisapproveMessage); 
                return Json(new { result = status }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new {result=status},JsonRequestBehavior.AllowGet);
            }
        }

        [CustomAuthorization("Employee")]
        [HttpPost]
        public async Task<JsonResult> AddEnrollment(int trainingID)
        {
            int userID = GetUserID();
            bool addEnrollmentStatus = _enrollmentService.AddEnrollment(userID, trainingID);
            if (addEnrollmentStatus)
            {
                await _enrollmentService.EmployeeSendMailToManagerForApplication(userID, trainingID);
                return Json(new { result = addEnrollmentStatus}, JsonRequestBehavior.AllowGet); 
            }
            else
            {
                return Json(new { result = addEnrollmentStatus },JsonRequestBehavior.AllowGet);
            }
        }


        [CustomAuthorization("Employee")]
        [HttpGet]
        public JsonResult GetFinalApprovedTrainings()
        {
            
            var list = _enrollmentService.GetEnrollments(GetUserID(), DataLibrary.Enum.Status.Approved, DataLibrary.Enum.Status.Approved);  //(FinalStatus, ManagerStatus)
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorization("Employee")]
        [HttpGet]
        public JsonResult GetManagerApprovedTrainings()
        {
            var list = _enrollmentService.GetEnrollments(GetUserID(), DataLibrary.Enum.Status.Processing, DataLibrary.Enum.Status.Approved);  //(FinalStatus, ManagerStatus)
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorization("Employee")]
        [HttpGet]
        public JsonResult GetPendingTrainings()
        {
            var list = _enrollmentService.GetEnrollments(GetUserID(), DataLibrary.Enum.Status.Processing, DataLibrary.Enum.Status.Processing);  //(FinalStatus, ManagerStatus)
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorization("Employee")]
        [HttpGet]
        public JsonResult GetDeclinedTrainings()
        {
            var list = _enrollmentService.GetDeclinedEnrollments(GetUserID());  
            return Json(list,JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorization("Manager")]
        [HttpGet]
        public JsonResult GetPendingEnrollments()
        {
            int ManagerID = GetUserID();
            var list = _enrollmentService.GetPendingEnrollments(ManagerID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorization("Manager")]
        [HttpPost]
        public JsonResult GetManagerApproveAndDisapproved(string Choice)
        {
            var list = _enrollmentService.GetManagerApproveAndDisapproved(Choice, GetUserID());
            return Json(list, JsonRequestBehavior.AllowGet);
        }


        [CustomAuthorization("Admin")]
        [HttpPost]
        public async Task<JsonResult> GetEmployeesAppliedForTraining(int trainingID)
        {
            var list = await _enrollmentService.GetEmployeesAppliedForTraining(trainingID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public int GetUserID()
        {
            IUser user = this.Session["CurrentUser"] as User;
            int userID = user.UserID;
            return userID;
        }

    }
}