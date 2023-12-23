﻿using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [HttpPost]
        public JsonResult AddEnrollment(int trainingID)
        {
            IUser user = this.Session["CurrentUser"] as User;
            int userID = user.UserID;
            bool status = _enrollmentService.AddEnrollment(userID, trainingID);
            return Json(new { result = status },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetFinalApprovedTrainings()
        {
            IUser user = this.Session["CurrentUser"] as User;
            int userID = user.UserID;
            var list = _enrollmentService.GetEnrollments(userID, DataLibrary.Enum.Status.Approved, DataLibrary.Enum.Status.Approved);  //(FinalStatus, ManagerStatus)
            return Json(list,JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetManagerApprovedTrainings()
        {
            IUser user = this.Session["CurrentUser"] as User;
            int userID = user.UserID;
            var list = _enrollmentService.GetEnrollments(userID,DataLibrary.Enum.Status.Processing, DataLibrary.Enum.Status.Approved);  //(FinalStatus, ManagerStatus)
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetPendingTrainings()
        {
            IUser user = this.Session["CurrentUser"] as User;
            int userID = user.UserID;
            var list = _enrollmentService.GetEnrollments(userID, DataLibrary.Enum.Status.Processing, DataLibrary.Enum.Status.Processing);  //(FinalStatus, ManagerStatus)
            return Json(list,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetDeclinedTrainings()
        {
            IUser user = this.Session["CurrentUser"] as User;
            int userID = user.UserID;
            var list = _enrollmentService.GetEnrollments(userID, DataLibrary.Enum.Status.Disapproved, DataLibrary.Enum.Status.Disapproved);  //(FinalStatus, ManagerStatus)
            return Json(list,JsonRequestBehavior.AllowGet);
        }
    }
}