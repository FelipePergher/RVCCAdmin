// <copyright file="PatientInformationCancerType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Domain;

namespace RVCC.Data.Models.RelationModels
{
    public class PatientInformationCancerType : IPatientInformationCancerType
    {
        public PatientInformationCancerType()
        {
        }

        public PatientInformationCancerType(CancerType cancerType)
        {
            CancerType = cancerType;
        }

        public int PatientInformationId { get; set; }

        public PatientInformation PatientInformation { get; set; }

        public int CancerTypeId { get; set; }

        public CancerType CancerType { get; set; }
    }
}
