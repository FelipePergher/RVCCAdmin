// <copyright file="Doctor.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models
{
    public class Doctor : RegisterData
    {
        public Doctor()
        {
        }

        public Doctor(string name)
        {
            Name = name;
        }

        public Doctor(string name, string crm)
        {
            Name = name;
            CRM = crm;
        }

        [Key]
        public int DoctorId { get; set; }

        public string Name { get; set; }

        public string CRM { get; set; }

        #region Relations

        public ICollection<PatientInformationDoctor> PatientInformationDoctors { get; set; }

        #endregion
    }
}