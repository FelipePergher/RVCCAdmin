// <copyright file="TreatmentType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Domain
{
    public class TreatmentType : RegisterData, ITreatmentType
    {
        public TreatmentType()
        {
        }

        public TreatmentType(string name, string note)
        {
            Name = name;
            Note = note;
        }

        #region ITreatmentType

        [Key]
        public int TreatmentTypeId { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        #endregion

        #region Relations

        public ICollection<PatientTreatmentType> PatientTreatmentTypes { get; set; }

        #endregion
    }
}