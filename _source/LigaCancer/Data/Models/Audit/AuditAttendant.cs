// <copyright file="AuditAttendant.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditAttendant : IAudit, IAttendant
    {
        [Key]
        public int AuditAttendantId { get; set; }

        #region IAttendant

        public int AttendantId { get; set; }

        public string Name { get; set; }

        public string CPF { get; set; }

        public string Phone { get; set; }

        public string Note { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}
