// <copyright file="AuditMedicine.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditMedicine : IAudit, IMedicine
    {
        [Key]
        public int AuditMedicineId { get; set; }

        #region IMedicine

        public int MedicineId { get; set; }

        public string Name { get; set; }

        #endregion

        #region Audit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}