// <copyright file="AuditAuxiliarAccessoryType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditAuxiliarAccessoryType : IAudit, IAuxiliarAccessoryType
    {
        [Key]
        public int AuditAuxiliarAccessoryTypeId { get; set; }

        #region IAuxiliarAccessoryType

        public int AuxiliarAccessoryTypeId { get; set; }

        public string Name { get; set; }

        #endregion

        #region Audit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}