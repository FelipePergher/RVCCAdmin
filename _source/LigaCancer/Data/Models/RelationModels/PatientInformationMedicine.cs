// <copyright file="PatientInformationMedicine.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Domain;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.RelationModels
{
    public class PatientInformationMedicine : IPatientInformationMedicine
    {
        public PatientInformationMedicine()
        {
        }

        public PatientInformationMedicine(Medicine medicine)
        {
            Medicine = medicine;
        }

        public int PatientInformationId { get; set; }

        public int MedicineId { get; set; }

        #region Relation

        [ForeignKey(nameof(PatientInformationId))]
        public PatientInformation PatientInformation { get; set; }

        [ForeignKey(nameof(MedicineId))]
        public Medicine Medicine { get; set; }

        #endregion
    }
}
