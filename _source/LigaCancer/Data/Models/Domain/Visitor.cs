// <copyright file="Visitor.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Domain
{
    public class Visitor : RegisterData, IVisitor
    {
        public Visitor()
        {
        }

        public Visitor(string name, string cpf, string phone)
        {
            Name = name;
            CPF = cpf;
            Phone = phone;
        }

        #region IVisitor

        [Key]
        public int VisitorId { get; set; }

        public string Name { get; set; }

        public string CPF { get; set; }

        public string Phone { get; set; }
        #endregion

        #region Relation

        public ICollection<VisitorAttendanceType> VisitorAttendanceTypes { get; set; }

        #endregion
    }
}