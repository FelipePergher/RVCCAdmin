﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.MedicalViewModels
{
    public class PatientInformationViewModel
    {
        public PatientInformationViewModel()
        {
            CancerTypes = new List<string>();
            Medicines = new List<string>();
            Doctors = new List<string>();
            TreatmentPlaces = new List<string>();
        }


        [Display(Name = "Tipos de Câncer")]
        public List<string> CancerTypes { get; set; }

        [Display(Name = "Médicos")]
        public List<string> Doctors { get; set; }

        [Display(Name = "Locais de tratamento")]
        public List<string> TreatmentPlaces { get; set; }

        [Display(Name = "Remédios")]
        public List<string> Medicines { get; set; }
    }
}
