using DataLibrary.BusinessLogic.BusinessLogicInterface;
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
    [CustomAuthorization("Admin")]
    public class AdminController : Controller
    {
        private readonly IUserService _userService;
        public AdminController(IUserService userService)
        {
            this._userService = userService;
        }
        

        public ActionResult AdminView()
        {
            return View();
        }
        public ActionResult AutomaticProcessingView()
        {
            return View();
        }
        public ActionResult AllUsersView()
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

        //admin only
        [HttpGet]
        public async Task<JsonResult> GetAllUsers()
        {
            var listOfAllUsers = await _userService.GetAll();
            return Json(listOfAllUsers, JsonRequestBehavior.AllowGet);
        }

        //admin only
        [HttpGet]
        public async Task<JsonResult> GetTotalNumberOfUserRecords()
        {
            var numberOfUsers = await _userService.GetTotalNumberOfUserRecords();
            return Json(numberOfUsers, JsonRequestBehavior.AllowGet);   
        }

        //admin only and maybe the user
        [HttpPost]
        public async Task<JsonResult> GetUserDetails(int UserID)
        {
            var UserDetails = await _userService.GetById(UserID);
            return Json(UserDetails, JsonRequestBehavior.AllowGet);

        }




    }
}