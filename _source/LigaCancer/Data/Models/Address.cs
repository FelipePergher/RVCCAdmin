// <copyright file="Address.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models
{
    public class Address : RegisterData
    {
        [Key]
        public int AddressId { get; set; }

        public string Street { get; set; }

        public string Neighborhood { get; set; }

        public string City { get; set; }

        public string HouseNumber { get; set; }

        public string Complement { get; set; }

        public string ObservationAddress { get; set; }

        public Enums.ResidenceType? ResidenceType { get; set; }

        public double MonthlyAmountResidence { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }
    }
}