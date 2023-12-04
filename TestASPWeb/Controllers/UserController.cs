using DataLibrary.BusinessLogic;
using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestASPWeb.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly DepartmentService _departmentService;

        public UserController(IUserService userService,DepartmentService departmentService)
        {
            _userService = userService;
            _departmentService = departmentService;
            
        }


        public ActionResult Login() => View();
        public ActionResult Register() => View();   


        [HttpPost]
        public JsonResult Authenticate(LoginUserViewModel loginUserViewModel)
        {
            if(!ModelState.IsValid)
            {
                return Json(new { result = false });
            }

                //get user from BL to see if user is authenticated or not ? 
                IUser user = _userService.Authenticate(loginUserViewModel);

                //If BL returns a null user, it means that this email does not exist , else user exists
                if (user == null)
                {
                    return Json(new { result = false });
                }

                //set up sessions
                this.Session["CurrentUser"] = user;
                this.Session["CurrentRole"] = user.Role;
                this.Session["CurrentUserId"] = user.UserID;

                //return json to home index controller
                return Json(new { result = true, url = Url.Action("Index", "Home") });
        }

        [HttpGet]
        public JsonResult GetDepartments()
        {
            return Json(new { listDepartments = _departmentService.GetAll() },JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetManagers()
        {
            return Json(new { listManagers = _userService.GetAllManagers() }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}