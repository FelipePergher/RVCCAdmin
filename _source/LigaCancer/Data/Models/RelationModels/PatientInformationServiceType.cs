// <copyright file="PatientInformationServiceType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.RelationModels
{
    public class PatientInformationServiceType : IPatientInformationServiceType
    {
        public PatientInformationServiceType()
        {
        }

        public PatientInformationServiceType(ServiceType serviceType)
        {
            ServiceType = serviceType;
        }

        public int PatientInformationId { get; set; }

        public int ServiceTypeId { get; set; }

        #region Relation

        [ForeignKey(nameof(PatientInformationId))]
        public PatientInformation PatientInformation { get; set; }

        [ForeignKey(nameof(PatientInformationId))]
        public ServiceType ServiceType { get; set; }

        #endregion
    }
}
