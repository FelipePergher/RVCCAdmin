// <copyright file="IStay.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;

namespace RVCC.Data.Interfaces
{
    public interface IStay
    {
        public int StayId { get; set; }

        public int? PatientId { get; set; }

        public string PatientName { get; set; }

        public string City { get; set; }

        public DateTime StayDateTime { get; set; }

        public string Note { get; set; }
    }
}