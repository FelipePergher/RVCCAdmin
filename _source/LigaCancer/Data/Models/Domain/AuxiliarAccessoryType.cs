// <copyright file="AuxiliarAccessoryType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Domain
{
    public class AuxiliarAccessoryType : RegisterData, IAuxiliarAccessoryType
    {
        public AuxiliarAccessoryType()
        {
        }

        public AuxiliarAccessoryType(string name)
        {
            Name = name;
        }

        #region IAuxiliarAccessoryType

        [Key]
        public int AuxiliarAccessoryTypeId { get; set; }

        public string Name { get; set; }

        #endregion

        #region Relations

        public ICollection<PatientAuxiliarAccessoryType> PatientAuxiliarAccessoryTypes { get; set; }

        #endregion
    }
}