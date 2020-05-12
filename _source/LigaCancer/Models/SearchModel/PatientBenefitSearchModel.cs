﻿// <copyright file="PatientBenefitSearchModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class PatientBenefitSearchModel
    {
        public PatientBenefitSearchModel()
        {
            DateFrom = DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy");
            DateTo = DateTime.Now.ToString("dd/MM/yyyy");
        }

        public string PatientId { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Benefício")]
        public string Benefit { get; set; }

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
