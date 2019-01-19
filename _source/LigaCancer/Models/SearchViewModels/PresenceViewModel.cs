using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static System.Net.Mime.MediaTypeNames;

namespace LigaCancer.Models.SearchViewModels
{
    public class PresenceViewModel
    {
        public PresenceViewModel()
        {
            Date = DateTime.Now;
            Time = new TimeSpan(DateTime.Now.Hour,DateTime.Now.Minute, 0);
            Patients = new List<SelectListItem>();
        }

        public int PresenceId { get; set; }

        [Display(Name = "Paciente"), Required(ErrorMessage = "Este campo é obrigatório.")]
        public string Patient { get; set; }

        [Display(Name = "Data"),
            DataType(DataType.Date),
            DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
            
        public DateTime Date { get; set; }

        [Display(Name = "Hora"),
            DisplayFormat(DataFormatString = @"{0:hh\:mm}", ApplyFormatInEditMode = true)]
        public TimeSpan Time { get; set; }

        public List<SelectListItem> Patients { get; set; }

    }
}
