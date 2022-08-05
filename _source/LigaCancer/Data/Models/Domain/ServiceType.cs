// <copyright file="ServiceType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Domain
{
    public class ServiceType : RegisterData, IServiceType
    {
        public ServiceType()
        {
        }

        public ServiceType(string name)
        {
            Name = name;
        }

        #region IServiceType

        [Key]
        public int ServiceTypeId { get; set; }

        public string Name { get; set; }

        #endregion

        #region Relations

        public ICollection<PatientInformationServiceType> PatientInformationServiceTypes { get; set; }

        #endregion
    }
}