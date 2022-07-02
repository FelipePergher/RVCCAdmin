// <copyright file="IAddress.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;

namespace RVCC.Data.Interfaces
{
    public interface IAddress
    {
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
    }
}