// <copyright file="Stay.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models
{
    public class Stay : RegisterData
    {
        [Key]
        public int StayId { get; set; }

        public int? PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        public string PatientName { get; set; }

        public string City { get; set; }

        public DateTime StayDateTime { get; set; }

        public string Note { get; set; }
    }
}
