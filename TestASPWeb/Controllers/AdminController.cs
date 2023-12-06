﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestASPWeb.Custom;

namespace TestASPWeb.Controllers
{
    [UserSessionAttribute]
    [CustomAuthorizationAttribute("admin")]//authorization
    public class AdminController : Controller
    {
        
        public ActionResult Index()
        {
            return View();
        }
    }
}