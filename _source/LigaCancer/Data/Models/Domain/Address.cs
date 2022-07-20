// <copyright file="Address.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.Domain
{
    public class Address : RegisterData, IAddress
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

        #region Relations

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }

        #endregion
    }
}