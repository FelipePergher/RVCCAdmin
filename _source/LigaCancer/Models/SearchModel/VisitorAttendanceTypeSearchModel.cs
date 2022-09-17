﻿// <copyright file="VisitorAttendanceTypeSearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class VisitorAttendanceTypeSearchModel
    {
        public VisitorAttendanceTypeSearchModel()
        {
            DateFrom = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");
            DateTo = DateTime.Now.ToString("dd/MM/yyyy");
        }

        public string VisitorId { get; set; }

        [Display(Name = "Visitante")]
        public string Name { get; set; }

        [Display(Name = "Atendente")]
        public string Attendant { get; set; }

        [Display(Name = "Atendimento")]
        public string Attendance { get; set; }

        [Display(Name = "Data Inicial")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        [DataType(DataType.Date)]
        public string DateFrom { get; set; }

        [Display(Name = "Data Final")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string DateTo { get; set; }
    }
}
