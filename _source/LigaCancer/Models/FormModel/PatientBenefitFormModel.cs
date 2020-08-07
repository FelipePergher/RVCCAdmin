// <copyright file="PatientBenefitFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class PatientBenefitFormModel
    {
        public PatientBenefitFormModel()
        {
            Date = DateTime.Now.ToString("dd/MM/yyyy");
        }

        [HiddenInput]
        public int PatientIdHidden { get; set; }

        [HiddenInput]
        public int BenefitIdHidden { get; set; }

        [Display(Name = "Paciente")]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string PatientId { get; set; }

        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string Date { get; set; }

        [Display(Name = "Benefício")]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Benefit { get; set; }

        [Range(1, 999999, ErrorMessage = "Quantidade deve ser maior que 0")]
        public int Quantity { get; set; }

        public List<SelectListItem> Patients { get; set; }

        public List<SelectListItem> BenefitsList { get; set; }
    }
}
