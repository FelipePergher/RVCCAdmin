// <copyright file="Benefit.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using RVCC.Data.Models.RelationModels;

namespace RVCC.Data.Models
{
    public class Benefit : RegisterData
    {
        public Benefit()
        {
        }

        public Benefit(string name)
        {
            Name = name;
        }

        [Key]
        public int BenefitId { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        #region Relations

        public ICollection<PatientBenefit> PatientBenefits { get; set; }

        #endregion
    }
}
