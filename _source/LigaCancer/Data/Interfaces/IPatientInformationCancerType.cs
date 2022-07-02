// <copyright file="IPatientInformationCancerType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface IPatientInformationCancerType
    {
        public int PatientInformationId { get; set; }

        public int CancerTypeId { get; set; }
    }
}