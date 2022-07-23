// <copyright file="IPatientInformationServiceType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface IPatientInformationServiceType
    {
        public int PatientInformationId { get; set; }

        public int ServiceTypeId { get; set; }
    }
}