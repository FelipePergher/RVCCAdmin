// <copyright file="ITreatmentType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface ITreatmentType
    {
        public int TreatmentTypeId { get; set; }

        public string Name { get; set; }

        public string Note { get; set; }
    }
}