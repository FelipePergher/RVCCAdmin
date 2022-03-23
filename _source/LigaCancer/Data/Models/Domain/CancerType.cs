// <copyright file="CancerType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Domain
{
    public class CancerType : RegisterData, ICancerType
    {
        public CancerType()
        {
        }

        public CancerType(string name)
        {
            Name = name;
        }

        #region ICancerType

        [Key]
        public int CancerTypeId { get; set; }

        public string Name { get; set; }

        #endregion

        #region Relations

        public ICollection<PatientInformationCancerType> PatientInformationCancerTypes { get; set; }

        #endregion
    }
}