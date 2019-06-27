using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class PresenceFormModel
    {
        public PresenceFormModel()
        {
            Date = DateTime.Now;
            Time = new TimeSpan(DateTime.Now.Hour, DateTime.Now.Minute, 0);
        }

        [Display(Name = "Paciente")]
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string PatientId { get; set; }

        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        [Display(Name = "Hora")]
        [DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan Time { get; set; }

        public List<SelectListItem> Patients { get; set; }

    }
}
