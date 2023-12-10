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
    public class TrainingController : Controller
    {
        private readonly ITrainingService _trainingService;


        public TrainingController(ITrainingService trainingService)
        {
            this._trainingService = trainingService;
        }

        public ActionResult Index()
        {
            return View();
        }


        public JsonResult getAll()
        {
            IEnumerable<ITraining> trainingList = _trainingService.GetAll();
            return Json(trainingList,JsonRequestBehavior.AllowGet);
        }

    }
}