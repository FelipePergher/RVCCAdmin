using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.ViewModel
{
    public class PresenceViewModel
    {
        public string Patient { get; set; }
        public string Date { get; set; }
        public string Hour { get; set; }
        public string Actions { get; set; }
    }
}
