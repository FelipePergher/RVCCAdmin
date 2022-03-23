// <copyright file="ITreatmentPlace.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface ITreatmentPlace
    {
        public int TreatmentPlaceId { get; set; }

        public string City { get; set; }
    }
}