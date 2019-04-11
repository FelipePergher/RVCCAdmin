﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class NaturalityFormModel
    {
        public NaturalityFormModel(){}

        public NaturalityFormModel(string patientId) => PatientId = patientId;

        [HiddenInput]
        public string PatientId { get; set; }

        [Display(Name = "Cidade")]
        public string City { get; set; }

        [Display(Name = "Estado")]
        public string State { get; set; }

        [Display(Name = "País")]
        public string Country { get; set; }
    }
}
