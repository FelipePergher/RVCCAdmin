// <copyright file="IPhone.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;

namespace RVCC.Data.Interfaces
{
    public interface IPhone
    {
        public int PhoneId { get; set; }

        public string Number { get; set; }

        public Enums.PhoneType? PhoneType { get; set; }

        public string ObservationNote { get; set; }

        public int PatientId { get; set; }
    }
}