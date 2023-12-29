using DataLibrary.BusinessLogic;
using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Enum;
using DataLibrary.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TestASPWeb.Custom;
using TestASPWeb.Enums;

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
        public ActionResult SelectLoginRole(String roles)
        {
            List<int> rolesList = roles.Split(',').Select(int.Parse).ToList();
            List<UserRole> userRoles = rolesList.ConvertAll(roleId => (UserRole)roleId);

            ViewBag.Roles = userRoles;
            return View();
        }

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
            this.Session["CurrentUserID"] = user.UserID;
            List<int> roleIds = _userService.GetRoleList(user.UserID);

            if(roleIds.Count > 1)
            {
                //meaning that this user has more that one role, we should allow that user to select which type of role he want to login as 
                return Json(new { pending = true ,url = Url.Action("SelectLoginRole","User",new {roles = string.Join(",",roleIds)})});
            }
            else
            {
                //only one role for this user, no need to ask him how to login as
                int roleId = roleIds.FirstOrDefault();
                UserRole userRole = (UserRole)Enum.ToObject(typeof(UserRole), roleId);
                this.Session["CurrentRole"] = userRole.ToString();

                if (userRole.ToString() == "Employee") return Json(new { result = true, url = Url.Action("EmployeeView", "Employee") });
                else if(userRole.ToString() == "Manager") return Json(new { result = true, url = Url.Action("ManagerView", "Manager") });
                else return Json(new { result = true, url = Url.Action("AdminView", "Admin") });

            }
        }

        [HttpPost]
        public JsonResult RedirectSelectedRole (string selectedRole)
        {
            if (!ModelState.IsValid) return Json(new { result = false });

            this.Session["CurrentRole"] = selectedRole;
            if (selectedRole == "Employee") return Json(new { result = true, url = Url.Action("EmployeeView", "Employee") }, JsonRequestBehavior.AllowGet);
            else if (selectedRole == "Manager") return Json(new { result = true, url = Url.Action("ManagerView", "Manager") }, JsonRequestBehavior.AllowGet);
            else return Json(new { result = true, url = Url.Action("AdminView", "Admin") }, JsonRequestBehavior.AllowGet);
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
            //since only employee can register
            IUser registeredUser = _userService.Register(user);
            if(registeredUser != null)
            {
                this.Session["CurrentUser"] = registeredUser;
                this.Session["CurrentRole"] = UserRole.Employee.ToString();
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