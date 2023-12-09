using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using DataLibrary;
using DataLibrary.BusinessLogic.BusinessLogicInterface;
using DataLibrary.Entities;
using DataLibrary.Entities.EntitiesInterface;
using DataLibrary.Repo;
using DataLibrary.Services;

namespace TestASPWeb.Controllers
{
    public class HomeController : Controller
    {

        //for testing purposes---------------------------------------- delete later
        
        private ITrainingService _trainingService;

        public HomeController(ITrainingService trainingService)
        {
            this._trainingService = trainingService;
            //var testingList = _trainingService.GetAll();
        }

        //for testing purposes-------------------------------------------------------



        public ActionResult Index()
        {

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult SignUp()
        {
            ViewBag.Message = "Your SignUp page.";

            return View();
        }



    }
}