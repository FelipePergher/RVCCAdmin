// <copyright file="Naturality.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.Domain
{
    public class Naturality : RegisterData, INaturality
    {
        [Key]
        public int NaturalityId { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public int PatientId { get; set; }

        #region Relation

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }

        #endregion
    }
}