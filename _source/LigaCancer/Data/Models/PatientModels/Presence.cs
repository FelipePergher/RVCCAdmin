using LigaCancer.Data.Models.ManyToManyModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using Microsoft.AspNetCore.Mvc.Rendering;
using static System.Net.Mime.MediaTypeNames;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Presence : RegisterData
    {
        [Key]
        public int PresenceId { get; set; }

        public Patient Patient { get; set; }

        public DateTime PresenceDateTime { get; set; }


    }



}
