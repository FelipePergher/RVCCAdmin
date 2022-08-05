// <copyright file="IPatientAuxiliarAccessoryType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;
using static RVCC.Business.Enums;

namespace RVCC.Data.Interfaces
{
    public interface IPatientAuxiliarAccessoryType
    {
        public int PatientAuxiliarAccessoryTypeId { get; set; }

        public int PatientId { get; set; }

        public int AuxiliarAccessoryTypeId { get; set; }

        public AuxiliarAccessoryTypeTime AuxiliarAccessoryTypeTime { get; set; }

        public DateTime DuoDate { get; set; }

        public string Note { get; set; }
    }
}