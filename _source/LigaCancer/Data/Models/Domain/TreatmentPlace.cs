// <copyright file="TreatmentPlace.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Domain
{
    public class TreatmentPlace : RegisterData, ITreatmentPlace
    {
        public TreatmentPlace()
        {
        }

        public TreatmentPlace(string city)
        {
            City = city;
        }

        #region ITreatmentPlace

        [Key]
        public int TreatmentPlaceId { get; set; }

        public string City { get; set; }

        #endregion

        #region Relations

        public ICollection<PatientInformationTreatmentPlace> PatientInformationTreatmentPlaces { get; set; }

        #endregion
    }
}