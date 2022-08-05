// <copyright file="AuditPatientAuxiliarAccessoryType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;
using static RVCC.Business.Enums;

namespace RVCC.Data.Models.Audit
{
    public class AuditPatientAuxiliarAccessoryType : IAudit, IPatientAuxiliarAccessoryType
    {
        [Key]
        public int AuditAuxiliarAccessoryTypeId { get; set; }

        #region IPatientAuxiliarAccessoryType

        public int PatientAuxiliarAccessoryTypeId { get; set; }

        public int PatientId { get; set; }

        public int AuxiliarAccessoryTypeId { get; set; }

        public AuxiliarAccessoryTypeTime AuxiliarAccessoryTypeTime { get; set; }

        public DateTime DuoDate { get; set; }

        public string Note { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}