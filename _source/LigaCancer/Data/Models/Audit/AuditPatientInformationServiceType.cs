﻿// <copyright file="AuditPatientInformationServiceType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditPatientInformationServiceType : IAudit, IPatientInformationServiceType
    {
        [Key]
        public int AuditPatientInformationServiceTypeId { get; set; }

        #region IPatientInformationServiceType

        public int PatientInformationId { get; set; }

        public int ServiceTypeId { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}