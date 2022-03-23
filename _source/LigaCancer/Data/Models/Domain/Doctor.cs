// <copyright file="Doctor.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Domain
{
    public class Doctor : RegisterData, IDoctor
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

        #region IDoctor

        [Key]
        public int DoctorId { get; set; }

        public string Name { get; set; }

        public string CRM { get; set; }

        #endregion

        #region Relations

        public ICollection<PatientInformationDoctor> PatientInformationDoctors { get; set; }

        #endregion
    }
}