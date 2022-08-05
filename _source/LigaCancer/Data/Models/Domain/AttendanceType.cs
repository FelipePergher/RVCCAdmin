// <copyright file="AttendanceType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Domain
{
    public class AttendanceType : RegisterData, IAttendanceType
    {
        public AttendanceType()
        {
        }

        public AttendanceType(string name)
        {
            Name = name;
        }

        #region IAttendanceType

        [Key]
        public int AttendanceTypeId { get; set; }

        public string Name { get; set; }

        #endregion

        #region Relation

        public ICollection<VisitorAttendanceType> VisitorAttendanceTypes { get; set; }

        #endregion
    }
}