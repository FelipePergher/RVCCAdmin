// <copyright file="Benefit.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Domain
{
    public class Benefit : RegisterData, IBenefit
    {
        public Benefit()
        {
        }

        public Benefit(string name)
        {
            Name = name;
        }

        #region IBenefit

        [Key]
        public int BenefitId { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }

        #endregion

        #region Relations

        public ICollection<PatientBenefit> PatientBenefits { get; set; }

        #endregion
    }
}
