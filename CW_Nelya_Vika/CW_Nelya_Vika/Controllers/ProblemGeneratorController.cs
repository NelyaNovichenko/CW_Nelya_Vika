﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using CW_Nelya_Vika.Models;
using CW_Nelya_Vika.Models.DB;
using CW_Nelya_Vika.Models.Graph_Initializers;
using CW_Nelya_Vika_Algorithms;


namespace CW_Nelya_Vika.Controllers
{
    public class ProblemGeneratorController : Controller
    {
        public ProblemGeneratorController()
        {
            if(GraphProblemDb.CurrentProblem == null)
                GraphProblemDb.CurrentProblem = new Problem();
        }
        
        public ActionResult ProblemGenerator()
        {
            List<Graph> graphsFromLists = new List<Graph>();
            var problems = GraphProblemDb.Problems;
            var graphs = problems.Select(p => p.GraphList);
            var graphLists = graphs.ToList();
            foreach (var graphList in graphLists)
            {
                graphsFromLists.AddRange(graphList);
            }

            var graphsFromDb = GraphProblemDb.Graphs.Where(g => !graphsFromLists.Contains(g)).ToList();

            return View(graphsFromDb);
        }

        [HttpPost]
        public ActionResult ReadFromFile()
        {
            if (HttpContext.Request.Files.AllKeys.Any())
            {
                var httpPostedFile = HttpContext.Request.Files[0];

                if (httpPostedFile != null)
                {
                    var index = httpPostedFile.FileName.LastIndexOf(@"\");
                    var shotName = httpPostedFile.FileName.Substring(index == -1 ? 0 : index);
                    var fileSavePath = HttpContext.Server.MapPath("~/UploadedFiles") + shotName;

                    httpPostedFile.SaveAs(fileSavePath);
                    try
                    {
                        IGraphInitializer graphInitializer = new GraphFromFile(fileSavePath);
                        GraphProblemDb.CurrentProblem.Graph = graphInitializer.Initialize();
                    }
                    catch (Exception e)
                    {
                        return Redirect("Error");

                    }

                }
            }

            return RedirectToAction("OutputEditGraph", GraphProblemDb.CurrentProblem.Graph);
        }

        [HttpPost]
        public ActionResult Generate(FormCollection fc)
        {
            string communityCount = fc.GetValue("communityCount").AttemptedValue,
                //minCommunityCount = fc.GetValue("minCommunityCount").AttemptedValue,
                //maxCommunityCount = fc.GetValue("maxCommunityCount").AttemptedValue,
                minWeightStr = fc.GetValue("minWeight").AttemptedValue,
                maxWeightStr = fc.GetValue("maxWeight").AttemptedValue;

            int commCount = Convert.ToInt32(communityCount is null ? "0" : communityCount);
            //int minCommCount = Convert.ToInt32(minCommunityCount is null ? "0" : minCommunityCount);
            //int maxCommCount = Convert.ToInt32(maxCommunityCount is null ? "0" : maxCommunityCount);
            int minWeight = Convert.ToInt32(minWeightStr is null ? "0" : minWeightStr);
            int maxWeight = Convert.ToInt32(maxWeightStr is null ? "0" : maxWeightStr);
            var problemClassification = ProblemClassification.Xs;
            switch (fc.GetValue("GraphClassification").AttemptedValue)
            {
                case "XS":
                    problemClassification = ProblemClassification.Xs;
                    break;
                case "S":
                    problemClassification = ProblemClassification.S;
                    break;
                case "M":
                    problemClassification = ProblemClassification.M;
                    break;
                case "L":
                    problemClassification = ProblemClassification.L;
                    break;
                case "XL":
                    problemClassification = ProblemClassification.Xl;
                    break;
            }
            IGraphInitializer graphInitializer = new GraphGenerator(problemClassification, commCount, /*minCommCount, maxCommCount*/ minWeight, maxWeight);
            GraphProblemDb.CurrentProblem.Graph = graphInitializer.Initialize();

            return RedirectToAction("OutputEditGraph", GraphProblemDb.CurrentProblem.Graph);
        }

        [HttpPost]
        public ActionResult ReadFromDb(FormCollection fc)
        {
            int gId = Convert.ToInt32(fc.GetValue("GraphId").AttemptedValue);
            GraphProblemDb.CurrentProblem.Graph = GraphProblemDb.GetGraph(gId);

            return RedirectToAction("OutputEditGraph", GraphProblemDb.CurrentProblem.Graph);
        }


        public ActionResult OutputEditGraph()
        {
            return View(GraphProblemDb.CurrentProblem.Graph);
        }

        [HttpPost]
        public ActionResult SaveToDb()
        {
            GraphProblemDb.AddGraph(GraphProblemDb.CurrentProblem.Graph);
            return View("OutputEditGraph", GraphProblemDb.CurrentProblem.Graph);
        }

        public ActionResult Solve(FormCollection fc)
        {
            //if (problem == null)
            //    problem = new Problem();
            //problem.Graph = graph;
            // GraphList communities = new GraphList();
            switch (fc.GetValue("Algorithm").AttemptedValue)
            {
                case "KernighanLin":
                    GraphProblemDb.CurrentProblem.Algorithm = Algorithm.KernighanLin;
                    IAlgorithm algorithm = new KernighanLin();
                    GraphProblemDb.CurrentProblem = algorithm.FindCommunityStructure(GraphProblemDb.CurrentProblem.Graph);
                    break;
                case "GirvanNewman":
                    GraphProblemDb.CurrentProblem.Algorithm = Algorithm.GirvanNewman;
                    algorithm = new GirvanNewman();
                    GraphProblemDb.CurrentProblem = algorithm.FindCommunityStructure(GraphProblemDb.CurrentProblem.Graph);
                    break;
            }
            //problem.GraphList = communities;

            //TODO: перейти на страничку и передать парам
            return RedirectToAction("GraphListResult", "GraphListResult", GraphProblemDb.CurrentProblem);
        }



    }
}