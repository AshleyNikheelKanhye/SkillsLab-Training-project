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
using TestASPWeb.Custom;

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

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Login","User");
        }

        [HttpPost]
        public JsonResult Authenticate(LoginUserViewModel loginUserViewModel)
        {
            if(!ModelState.IsValid) return Json(new { result = false });
            IUser user = _userService.Authenticate(loginUserViewModel);
            if (user == null) return Json(new { result = false });

            this.Session["CurrentUser"] = user;
            this.Session["CurrentRole"] = user.Role;
            this.Session["CurrentUserID"] = user.UserID;

            if(user.Role == "employee") return Json(new { result = true, url = Url.Action("EmployeeView", "Employee") });
            else if(user.Role == "manager") return Json(new { result = true, url = Url.Action("Index", "Manager") });
            else return Json(new { result = true, url = Url.Action("Index", "Admin") });
        }

        [HttpGet]
        public JsonResult GetDepartments()
        {
            var listDept = _departmentService.GetAll();
            if (listDept !=null)
            {
                return Json(new { listDepartments = listDept }, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = true },JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetManagers()
        {
            var list = _userService.GetAllManagers();
            if(list !=null)
            {
                return Json(new { listManagers = list}, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new { error = true }, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpPost]
        public JsonResult CheckUserExist(CheckUserExistViewModel checkUserExistViewModel)
        {
            bool isExist = _userService.CheckUserExist(checkUserExistViewModel);
            if (isExist) return Json(new { result = true });
            else return Json(new { result = false });
        }



        [HttpPost]
        public JsonResult Register(User user)
        {
            user.Role = "employee"; //since only employee can register
            IUser registeredUser = _userService.Register(user);
            if(registeredUser != null)
            {
                this.Session["CurrentUser"] = registeredUser;
                this.Session["CurrentRole"] = registeredUser.Role;
                this.Session["CurrentUserID"] = registeredUser.UserID;
                return Json(new { result = registeredUser });
            }
            else
            {
                return Json(new { result = false });
            }
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}