using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestASPWeb.Custom;

namespace TestASPWeb.Controllers
{
    [UserSessionAttribute]
    [CustomAuthorizationAttribute("Admin")]//authorization
    public class AdminController : Controller
    {
        

        public ActionResult AdminView()
        {
            return View();
        }
        public ActionResult AutomaticProcessingView()
        {
            return View();
        }
        public ActionResult ManageRolesView()
        {
            return View();
        }
        public ActionResult ManageTrainingsView()
        {
            return View();

        }
        public ActionResult ViewTrainingsView()
        {
            return View();
        }
        public ActionResult AddNewTrainingView()
        {
            return View();  
        }
    }
}