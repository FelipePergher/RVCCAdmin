// <copyright file="AuditCancerType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditCancerType : IAudit, ICancerType
    {
        [Key]
        public int AuditCancerTypeId { get; set; }

        #region ICancerType

        public int CancerTypeId { get; set; }

        public string Name { get; set; }

        #endregion

        #region Audit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}