// <copyright file="Medicine.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models
{
    public class Medicine : RegisterData
    {
        public Medicine()
        {
        }

        public Medicine(string name)
        {
            Name = name;
        }

        [Key]
        public int MedicineId { get; set; }

        public string Name { get; set; }

        #region Relations

        public ICollection<PatientInformationMedicine> PatientInformationMedicines { get; set; }

        #endregion
    }
}