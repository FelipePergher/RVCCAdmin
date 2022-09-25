// <copyright file="AuditTreatmentType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditTreatmentType : IAudit, ITreatmentType
    {
        [Key]
        public int AuditTreatmentTypeId { get; set; }

        #region ITreatmentType

        public int TreatmentTypeId { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        #endregion

        #region Audit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}