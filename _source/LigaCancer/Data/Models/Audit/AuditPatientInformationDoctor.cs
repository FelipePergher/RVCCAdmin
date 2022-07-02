// <copyright file="AuditPatientInformationDoctor.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditPatientInformationDoctor : IAudit, IPatientInformationDoctor
    {
        [Key]
        public int AuditPatientInformationDoctorId { get; set; }

        #region IPatientInformationDoctor

        public int PatientInformationId { get; set; }

        public int DoctorId { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}