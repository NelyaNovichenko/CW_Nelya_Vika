﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CW_Nelya_Vika.Models
{
    /// <summary>
    /// Задає розмір графу:
    /// - Xs - 0...
    /// - S - ...
    /// - M - ...
    /// - L - ...
    /// - Xl - ...
    /// </summary>
    public enum ProblemClassification : byte
    {
        Xs, S, M, L, Xl
    }

    /// <summary>
    /// Клас, що генерує випадкові ребра та вершини,
    /// враховуючи розмірність
    /// </summary>
    public class GraphGenerator : IGraphInitializer
    {
        /// <summary>
        /// Розмірність графу
        /// </summary>
        public ProblemClassification ProblemClassification { get; set; }

        /// <summary>
        /// Constructor gets Problem Classification. 
        /// Default value is 'S'.
        /// </summary>
        /// <param name="pC"></param>
        public GraphGenerator(ProblemClassification pC = ProblemClassification.S)
        {
            ProblemClassification = pC;
        }
        /// <summary>
        /// Генерація графу заданої размірності
        /// </summary>
        public Graph Initialize()
        {
            Graph graph = new Graph();
            int vertexCount = 0;
            switch (ProblemClassification)
            {
                case ProblemClassification.Xs:
                    vertexCount = 15;
                    break;
                case ProblemClassification.S:
                    vertexCount = 30;
                    break;
                case ProblemClassification.M:
                    vertexCount = 60;
                    break;
                case ProblemClassification.L:
                    vertexCount = 120;
                    break;
                case ProblemClassification.Xl:
                    vertexCount = 240;
                    break;
            }

            Random r = new Random();
            for (int i = 1; i <= vertexCount; i++)
            {
                int edgeCount = r.Next(1, 4);
                for (int j = 0; j < edgeCount; j++)
                {
                    int vertOutLabel = i;
                    int vertInLabel = i;
                    do
                    {
                        vertInLabel = r.Next(1, vertexCount);
                    } while (vertOutLabel == vertInLabel);

                    int weight = r.Next(1, 10);

                    Vertex vertexA = graph.FindNode(vertOutLabel, true);
                    Vertex vertexB = graph.FindNode(vertInLabel, true);

                    graph.CreateLink(vertexA, vertexB, weight);
                }
            }
            return graph;
        }
    }
}