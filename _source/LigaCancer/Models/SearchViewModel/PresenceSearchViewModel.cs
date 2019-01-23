using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace LigaCancer.Models.SearchViewModel
{
    public class PresenceSearchViewModel
    {
        public PresenceSearchViewModel()
        {
            Date = DateTime.Now;        
        }

        [Display(Name = "Nome")]
        public string Patient { get; set; }

        [Display(Name = "Data"), 
            DataType(DataType.Date), 
            DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }

        public List<Presence> Presences { get; set; }

        public List<SelectListItem> Patients { get; set; }
    }
}
