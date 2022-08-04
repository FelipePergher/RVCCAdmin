// <copyright file="VisitorAttendance.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.Domain;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.RelationModels
{
    public class VisitorAttendance : RegisterData, IVisitorAttendanceType
    {
        public VisitorAttendance()
        {
        }

        public VisitorAttendance(AttendanceType attendanceType)
        {
            AttendanceType = attendanceType;
        }

        #region IVisitorAttendanceType

        [Key]
        public int VisitorAttendanceTypeId { get; set; }

        public int VisitorId { get; set; }

        public int AttendanceTypeId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public string Observation { get; set; }

        #endregion

        #region Relations

        [ForeignKey(nameof(VisitorId))]
        public Visitor Visitor { get; set; }

        [ForeignKey(nameof(AttendanceTypeId))]
        public AttendanceType AttendanceType { get; set; }
        #endregion
    }
}
