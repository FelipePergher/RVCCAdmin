// <copyright file="IBenefit.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface IBenefit
    {
        public int BenefitId { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }
    }
}