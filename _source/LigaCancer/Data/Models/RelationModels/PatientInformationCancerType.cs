﻿// <copyright file="PatientInformationCancerType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

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

        public int CancerTypeId { get; set; }

        #region Relation

        [ForeignKey(nameof(PatientInformationId))]
        public PatientInformation PatientInformation { get; set; }

        [ForeignKey(nameof(PatientInformationId))]
        public CancerType CancerType { get; set; }

        #endregion
    }
}
