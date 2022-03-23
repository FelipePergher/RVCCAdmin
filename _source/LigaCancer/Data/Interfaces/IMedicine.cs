// <copyright file="IMedicine.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface IMedicine
    {
        public int MedicineId { get; set; }

        public string Name { get; set; }
    }
}