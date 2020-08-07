// <copyright file="StayFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class StayFormModel
    {
        public StayFormModel()
        {
            Date = DateTime.Now.ToString("dd/MM/yyyy");
        }

        [Display(Name = "Paciente")]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string PatientId { get; set; }

        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string Date { get; set; }

        [Display(Name = "Cidade")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string City { get; set; }

        [Display(Name = "Notas")]
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Note { get; set; }

        public List<SelectListItem> Patients { get; set; }
    }
}
