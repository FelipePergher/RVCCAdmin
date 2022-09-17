// <copyright file="Attendant.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Domain
{
    public class Attendant : RegisterData, IAttendant
    {
        public Attendant()
        {
        }

        public Attendant(string name, string cpf, string phone, string note)
        {
            Name = name;
            CPF = cpf;
            Phone = phone;
            Note = note;
        }

        #region IAttendant

        [Key]
        public int AttendantId { get; set; }

        public string Name { get; set; }

        public string CPF { get; set; }

        public string Phone { get; set; }

        public string Note { get; set; }

        #endregion

        #region Relation

        public ICollection<VisitorAttendanceType> VisitorAttendanceTypes { get; set; }

        #endregion
    }
}