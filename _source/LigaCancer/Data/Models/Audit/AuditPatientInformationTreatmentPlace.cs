// <copyright file="AuditPatientInformationTreatmentPlace.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditPatientInformationTreatmentPlace : IAudit, IPatientInformationTreatmentPlace
    {
        [Key]
        public int AuditPatientInformationTreatmentPlaceId { get; set; }

        #region IPatientInformationTreatmentPlace

        public int PatientInformationId { get; set; }

        public int TreatmentPlaceId { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}