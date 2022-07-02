// <copyright file="AuditPatientInformationCancerType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditPatientInformationCancerType : IAudit, IPatientInformationCancerType
    {
        [Key]
        public int AuditPatientInformationCancerTypeId { get; set; }

        #region IPatientInformationCancerType

        public int PatientInformationId { get; set; }

        public int CancerTypeId { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}