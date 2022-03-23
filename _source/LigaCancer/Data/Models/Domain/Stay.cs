// <copyright file="Stay.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.Domain
{
    public class Stay : RegisterData, IStay
    {
        #region IStay
        [Key]
        public int StayId { get; set; }

        public int? PatientId { get; set; }

        public string PatientName { get; set; }

        public string City { get; set; }

        public DateTime StayDateTime { get; set; }

        public string Note { get; set; }

        #endregion

        #region Relations

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        #endregion
    }
}
