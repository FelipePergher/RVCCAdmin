﻿using LigaCancer.Code;
using System;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.SearchModel
{
    public class PresenceSearchModel
    {
        public PresenceSearchModel()
        {
            DateFrom = DateTime.Now.AddDays(-7);
            DateTo = DateTime.Now;
        }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Data Inicial")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateFrom { get; set; }

        [Display(Name = "Data Final")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateTo { get; set; }
    }
}