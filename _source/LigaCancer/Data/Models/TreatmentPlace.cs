// <copyright file="TreatmentPlace.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models
{
    public class TreatmentPlace : RegisterData
    {
        public TreatmentPlace()
        {
        }

        public TreatmentPlace(string city, ApplicationUser user)
        {
            City = city;
            CreatedBy = user.Name;
        }

        [Key]
        public int TreatmentPlaceId { get; set; }

        public string City { get; set; }

        #region Relations

        public ICollection<PatientInformationTreatmentPlace> PatientInformationTreatmentPlaces { get; set; }

        #endregion
    }
}