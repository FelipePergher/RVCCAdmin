// <copyright file="PatientInformationMedicine.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Models.Domain;

namespace RVCC.Data.Models.RelationModels
{
    public class PatientInformationMedicine
    {
        public PatientInformationMedicine()
        {
        }

        public PatientInformationMedicine(Medicine medicine)
        {
            Medicine = medicine;
        }

        public int PatientInformationId { get; set; }

        public PatientInformation PatientInformation { get; set; }

        public int MedicineId { get; set; }

        public Medicine Medicine { get; set; }
    }
}
