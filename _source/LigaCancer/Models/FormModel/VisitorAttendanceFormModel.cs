// <copyright file="VisitorAttendanceFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class VisitorAttendanceFormModel
    {
        public VisitorAttendanceFormModel()
        {
            Date = DateTime.Now.ToString("dd/MM/yyyy");
        }

        [HiddenInput]
        public int VisitorIdHidden { get; set; }

        [HiddenInput]
        public int AttendanceTypeIdHidden { get; set; }

        [Display(Name = "Visitante")]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string VisitorId { get; set; }

        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string Date { get; set; }

        [Display(Name = "Tipo de Atendimento")]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Attendance { get; set; }

        [Display(Name = "Observação")]
        [Range(1, 999999, ErrorMessage = "Quantidade deve ser maior que 0")]
        public string Observation { get; set; }

        public List<SelectListItem> Visitors { get; set; }

        public List<SelectListItem> AttendanceTypes { get; set; }
    }
}
