using DataLibrary.BusinessLogic.BusinessLogicInterface;
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


    }
}