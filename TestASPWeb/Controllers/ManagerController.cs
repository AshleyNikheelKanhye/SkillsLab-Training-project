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
    [CustomAuthorization("Manager")]
    public class ManagerController : Controller
    {

        private readonly IUserService _userService;
        public ManagerController(IUserService userService)
        {
            _userService = userService;
        }

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ManagerView()
        {
            return View();
        }

        public ActionResult ApproveRequestView()
        {
            return View();
        }

        //manager only
        public async Task<JsonResult> GetEmployeesUnderManager()
        {
            var list = await _userService.GetEmployeesUnderManager((int)this.Session["CurrentUserID"]);
            return Json(list, JsonRequestBehavior.AllowGet);
        }



    }
}