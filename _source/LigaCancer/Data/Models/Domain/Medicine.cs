// <copyright file="Medicine.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Domain
{
    public class Medicine : RegisterData, IMedicine
    {
        public Medicine()
        {
        }

        public Medicine(string name)
        {
            Name = name;
        }

        #region IMedicine

        [Key]
        public int MedicineId { get; set; }

        public string Name { get; set; }

        #endregion

        #region Relations

        public ICollection<PatientInformationMedicine> PatientInformationMedicines { get; set; }

        #endregion
    }
}