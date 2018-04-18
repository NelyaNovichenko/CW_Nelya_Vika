﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace CW_Nelya_Vika.Models
{
    /// <summary>
    /// Перелік реалізованих алгоритмів
    /// </summary>
    public enum Algorithm : byte
    {
        GirvanNewman,
        KernighanLin
    }

    /// <summary>
    /// Клас, що описує модель поточної проблеми/моделі
    /// Містить граф та його поділи різними алгоритмами
    /// </summary>
    public class Problem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        //public int GraphId { get; set; }
        public Graph Graph { get; set; }

        //public int ResultId { get; set; }
        public Result Result { get; set; }

        public Algorithm Algorithm { get; set; }

    }
}