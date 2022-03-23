// <copyright file="AuditTreatmentPlace.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditTreatmentPlace : IAudit, ITreatmentPlace
    {
        [Key]
        public int AuditTreatmentPlaceId { get; set; }

        #region ITreatmentPlace

        public int TreatmentPlaceId { get; set; }

        public string City { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}