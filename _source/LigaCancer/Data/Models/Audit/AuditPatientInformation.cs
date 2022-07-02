// <copyright file="AuditPatientInformation.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditPatientInformation : IAudit, IPatientInformation
    {
        [Key]
        public int AuditPatientInformationId { get; set; }

        #region IPatientInformation

        public int PatientInformationId { get; set; }

        public DateTime TreatmentBeginDate { get; set; }

        public int PatientId { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}