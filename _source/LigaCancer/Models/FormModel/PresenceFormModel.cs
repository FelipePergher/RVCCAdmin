// <copyright file="PresenceFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class PresenceFormModel
    {
        public PresenceFormModel()
        {
            Date = DateTime.Now.ToString("dd/MM/yyyy");
            Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
        }

        [Display(Name = "Paciente")]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string PatientId { get; set; }

        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string Date { get; set; }

        [Display(Name = "Hora")]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan Time { get; set; }

        public List<SelectListItem> Patients { get; set; }
    }
}
