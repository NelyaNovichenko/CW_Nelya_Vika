﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CW_Nelya_Vika.Models;
using CW_Nelya_Vika.Models.DB;

namespace CW_Nelya_Vika.Controllers
{
    public class OutputResultController : Controller
    {
        //db = new db
        //db.Problems
        //private GraphProblemDb db = new GraphProblemDb();
        // GET: OutputResult
        public ActionResult OutputResult()
        {
            //var problemsFromDb = GraphProblemDb.Problems.ToList();
            return View();
        }
    }
}