using DataLibrary.BusinessLogic;
using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace TestASPWeb.Controllers
{
    public class TrainingController : Controller
    {
        private readonly ITrainingService _trainingService;
        private readonly IPrerequisiteService _prerequisiteService;


        public TrainingController(ITrainingService trainingService, IPrerequisiteService prerequisiteService)
        {
            this._trainingService = trainingService;
            this._prerequisiteService = prerequisiteService;
        }

        public ActionResult Index()
        {
            return View();
        }

        //admin as well
        [HttpGet]
        public JsonResult getAll()
        {
            IEnumerable<ITraining> trainingList = _trainingService.GetAll();
            return Json(trainingList,JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllElligilbe()
        {
            User thisUser = this.Session["CurrentUser"] as User;
            IEnumerable<ITraining> trainingList = _trainingService.GetAllElligible(thisUser.UserID);
            return Json(trainingList, JsonRequestBehavior.AllowGet);
        }


        //admin also
        [HttpPost]
        public JsonResult GetPrerequisites(int trainingID)
        {
            var list = _prerequisiteService.GetPrerequisites(trainingID);
            return Json(new { result = list },JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllTrainingWithPrerequisitesAndDepartments()
        {
            var list = _trainingService.GetAllPrerequisitesAndDepartments();
            return Json(list,JsonRequestBehavior.AllowGet);
        }



        //manager only 
        [HttpPost]
        public async Task<JsonResult> AddTraining(AddTrainingViewModel addTrainingViewModel)
        {
            bool insertResult = await _trainingService.Add(addTrainingViewModel);
            return Json(new { result = insertResult },JsonRequestBehavior.AllowGet);
        }

        //admin only
        [HttpGet]
        public async Task<JsonResult> GetUnprocessedTrainings()
        {
            var list = await _trainingService.GetUnprocessedTrainings();
            return Json(list ,JsonRequestBehavior.AllowGet);  
        }

        //admin only
        [HttpGet]
        public async Task<JsonResult> GetCompletedTrainings()
        {
            var list = await _trainingService.GetCompletedTrainings();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //admin only
        [HttpGet]
        public async Task<JsonResult> GetDeletedTrainings()
        {
            var list = await _trainingService.GetDeletedTrainings();
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //admin only
        [HttpPost]
        public async Task<JsonResult> GetSelectedEmployees(int TrainingID)
        {
            var list = await _trainingService.GetSelectedEmployees(TrainingID);
            return Json(list, JsonRequestBehavior.AllowGet);
        }

        //admin only
        [HttpPost]
        public async Task<JsonResult> GenerateFinalListOfSelectedEmployees(int trainingId)
        {
            var obj = await _trainingService.GenerateFinalListOfSelectedEmployees(trainingId);
            return Json(obj, JsonRequestBehavior.AllowGet);
        }

        //admin only
        [HttpPost]
        public async Task<JsonResult> ConfirmAutomaticSelection(int trainingId)
        {
            bool result = await _trainingService.ConfirmAutomaticSelection(trainingId);
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        //admin only
        [HttpGet]
        public async Task<JsonResult> getUpcomings()
        {
            var list = await _trainingService.getUpcomings();
            return Json(list, JsonRequestBehavior.AllowGet);    
        }

        //admin only
        [HttpPost]
        public async Task<JsonResult> GetTrainingToUpdateDetails(int trainingID)
        {
            var list = await _trainingService.GetTrainingToUpdateDetails(trainingID);
            return Json(list, JsonRequestBehavior.AllowGet);

        }

        //admin only
        [HttpPost]
        public async Task<JsonResult> UpdateTraining(UpdateTrainingViewModel formUpdateResult)
        {
            bool updateStatus = await _trainingService.Update(formUpdateResult);
            return Json(new { result = updateStatus }, JsonRequestBehavior.AllowGet);
        }

        //admin only
        [HttpPost]
        public async Task<JsonResult> Delete(int trainingID)
        {
            bool deleteStatus = await _trainingService.Delete(trainingID);
            return Json(deleteStatus, JsonRequestBehavior.AllowGet);
        }



    }
}