// <copyright file="AuditNaturality.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditNaturality : IAudit, INaturality
    {
        [Key]
        public int AuditNaturalityId { get; set; }

        #region INaturality

        public int NaturalityId { get; set; }

        public string City { get; set; }

        public string State { get; set; }

        public string Country { get; set; }

        public int PatientId { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}