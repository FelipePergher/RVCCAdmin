// <copyright file="IVisitorAttendanceType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;

namespace RVCC.Data.Interfaces
{
    public interface IVisitorAttendanceType
    {
        public int VisitorAttendanceTypeId { get; set; }

        public int VisitorId { get; set; }

        public int AttendantId { get; set; }

        public int AttendanceTypeId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public string Observation { get; set; }
    }
}