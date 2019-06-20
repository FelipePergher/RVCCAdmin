﻿using LigaCancer.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class TreatmentPlace : RegisterData
    {
        public TreatmentPlace() { }

        public TreatmentPlace(string city, ApplicationUser user)
        {
            City = city;
            UserCreated = user;
        }

        [Key]
        public int TreatmentPlaceId { get; set; }

        [Display(Name = "Cidade")]
        public string City { get; set; }

        #region Relations

        public ICollection<PatientInformationTreatmentPlace> PatientInformationTreatmentPlaces { get; set; }
 
        #endregion
    }
}