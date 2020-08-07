// <copyright file="CancerType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models
{
    public class CancerType : RegisterData
    {
        public CancerType()
        {
        }

        public CancerType(string name, ApplicationUser user)
        {
            Name = name;
            CreatedBy = user.Name;
        }

        [Key]
        public int CancerTypeId { get; set; }

        public string Name { get; set; }

        #region Relations

        public ICollection<PatientInformationCancerType> PatientInformationCancerTypes { get; set; }

        #endregion
    }
}