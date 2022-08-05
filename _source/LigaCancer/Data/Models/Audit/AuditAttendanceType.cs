// <copyright file="AuditAttendanceType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditAttendanceType : IAudit, IAttendanceType
    {
        [Key]
        public int AuditAttendanceTypeId { get; set; }

        #region IAttendanceType

        public int AttendanceTypeId { get; set; }

        public string Name { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}
