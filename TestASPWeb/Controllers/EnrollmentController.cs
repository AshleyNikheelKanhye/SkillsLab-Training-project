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

namespace TestASPWeb.Controllers
{
    public class EnrollmentController : Controller
    {
        private readonly IEnrollmentService _enrollmentService;
       
        public EnrollmentController(IEnrollmentService enrollmentService)
        {
            this._enrollmentService = enrollmentService;
        
        }

        public ActionResult Index()
        {
            return View();
        }


        //managerOnly
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





        [HttpPost]
        public async Task<JsonResult> AddEnrollment(int trainingID)
        {
            int userID = GetUserID();
            bool addEnrollmentStatus = _enrollmentService.AddEnrollment(userID, trainingID);
            if (addEnrollmentStatus)
            {
                //await to insert notification to the manager of the user that the user has applied for this training.
                await _enrollmentService.EmployeeSendMailToManagerForApplication(userID, trainingID);
                return Json(new { result = addEnrollmentStatus}, JsonRequestBehavior.AllowGet); 
            }
            else
            {
                return Json(new { result = addEnrollmentStatus },JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public async Task<JsonResult> TestingEmailService(int trainingID) //test function to test if manager gets email.
        {
            int userID = GetUserID();
            await _enrollmentService.SendTestMail();
            return Json(new {result = "ok"}, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFinalApprovedTrainings()
        {
            int userID = GetUserID();
            var list = _enrollmentService.GetEnrollments(userID, DataLibrary.Enum.Status.Approved, DataLibrary.Enum.Status.Approved);  //(FinalStatus, ManagerStatus)
            return Json(list,JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetManagerApprovedTrainings()
        {
            int userID = GetUserID();
            var list = _enrollmentService.GetEnrollments(userID,DataLibrary.Enum.Status.Processing, DataLibrary.Enum.Status.Approved);  //(FinalStatus, ManagerStatus)
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPendingTrainings()
        {
            int userID = GetUserID();
            var list = _enrollmentService.GetEnrollments(userID, DataLibrary.Enum.Status.Processing, DataLibrary.Enum.Status.Processing);  //(FinalStatus, ManagerStatus)
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDeclinedTrainings()
        {
            int userID = GetUserID();
            var list = _enrollmentService.GetDeclinedEnrollments(userID);  
            return Json(list,JsonRequestBehavior.AllowGet);
        }


        //manager only
        [HttpGet]
        public JsonResult GetPendingEnrollments()
        {
            int ManagerID = GetUserID();
            var list = _enrollmentService.GetPendingEnrollments(ManagerID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //managerOnly
        [HttpPost]
        public JsonResult GetManagerApproveAndDisapproved(string Choice)
        {
            var list = _enrollmentService.GetManagerApproveAndDisapproved(Choice, GetUserID());
            return Json(list, JsonRequestBehavior.AllowGet);
        }



        //Admin only
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