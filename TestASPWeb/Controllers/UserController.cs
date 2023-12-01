using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities;
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

        public UserController(IUserService userService)
        {
            _userService = userService;
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
                User user = (User)_userService.Authenticate(loginUserViewModel);

                //If BL returns a null user, it means that this email does not exist , else user exists
                if (user == null)
                {
                    return Json(new { result = false });
                }

                //set up sessions
                this.Session["CurrentUser"] = user;
                this.Session["CurrentRole"] = user.Role;
                this.Session["CurrentUserId"] = user.Id;

                //return json to home index controller
                return Json(new { result = true, url = Url.Action("Index", "Home") });

           
    

        }



        public ActionResult Index()
        {
            return View();
        }












    }
}