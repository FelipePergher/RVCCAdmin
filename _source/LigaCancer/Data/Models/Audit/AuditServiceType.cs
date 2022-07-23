// <copyright file="AuditServiceType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditServiceType : IAudit, IServiceType
    {
        [Key]
        public int AuditServiceTypeId { get; set; }

        #region IServiceType

        public int ServiceTypeId { get; set; }

        public string Name { get; set; }

        #endregion

        #region Audit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}