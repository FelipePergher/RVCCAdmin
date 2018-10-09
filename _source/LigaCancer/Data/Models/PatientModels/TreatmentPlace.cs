﻿using LigaCancer.Data.Models.ManyToManyModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class TreatmentPlace : RegisterData
    {
        [Key]
        public int TreatmentPlaceId { get; set; }

        [Display(Name = "Cidade")]
        public string City { get; set; }

        #region Relations

        public ICollection<PatientInformationTreatmentPlace> PatientInformationTreatmentPlaces { get; set; }
 
        #endregion
    }
}