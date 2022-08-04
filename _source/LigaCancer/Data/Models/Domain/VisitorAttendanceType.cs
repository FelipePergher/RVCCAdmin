// <copyright file="VisitorAttendanceType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.Domain
{
    public class VisitorAttendanceType : RegisterData, IVisitorAttendanceType
    {
        public VisitorAttendanceType()
        {
        }

        #region IVisitorAttendanceType

        [Key]
        public int VisitorAttendanceTypeId { get; set; }

        public DateTime AttendanceDate { get; set; }

        public string Observation { get; set; }

        #endregion

        #region Relation

        public int AttendanceTypeId { get; set; }

        [ForeignKey(nameof(AttendanceTypeId))]
        public AttendanceType AttendanceType { get; set; }

        public int VisitorId { get; set; }

        [ForeignKey(nameof(VisitorId))]
        public Visitor Visitor { get; set; }

        #endregion
    }
}