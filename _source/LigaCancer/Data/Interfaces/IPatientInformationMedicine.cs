// <copyright file="IPatientInformationMedicine.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface IPatientInformationMedicine
    {
        public int PatientInformationId { get; set; }

        public int MedicineId { get; set; }
    }
}