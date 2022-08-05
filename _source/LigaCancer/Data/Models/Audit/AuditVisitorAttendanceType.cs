// <copyright file="AuditVisitorAttendanceType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditVisitorAttendanceType : IAudit, IVisitorAttendanceType
    {
        [Key]
        public int AuditVisitorAttendanceTypeId { get; set; }

        #region IVisitorAttendanceType

        public int VisitorAttendanceTypeId { get; set; }

        public int VisitorId { get; set; }

        public int AttendanceTypeId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public string Observation { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}
