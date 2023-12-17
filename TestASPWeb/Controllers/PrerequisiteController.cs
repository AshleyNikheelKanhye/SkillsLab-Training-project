using DataLibrary.BusinessLogic.BusinessLogicInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TestASPWeb.Controllers
{
    public class PrerequisiteController : Controller
    {
        private readonly IPrerequisiteService _prerequisiteService;

        public PrerequisiteController(IPrerequisiteService prerequisiteService)
        {
            this._prerequisiteService = prerequisiteService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public JsonResult GetPrerequisitesNotInEmployee(int userID)
        {
            var listPrerequisites = _prerequisiteService.GetPrerequisitesNotInEmployee(userID);
            if(listPrerequisites != null)
            {
                return Json(new { listQualifications =listPrerequisites},JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(new {error = "error" });
            }
        }





    }
}