// <copyright file="PatientExpenseType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.RelationModels
{
    public class PatientExpenseType : IPatientExpenseType
    {
        public PatientExpenseType()
        {
        }

        public PatientExpenseType(ExpenseType expenseType, Patient patient, double value)
        {
            Patient = patient;
            ExpenseType = expenseType;
            Value = value;
        }

        [Key]
        public int PatientExpenseTypeId { get; set; }

        public double Value { get; set; }

        public int PatientId { get; set; }

        public int ExpenseTypeId { get; set; }

        #region Relation

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        [ForeignKey(nameof(ExpenseTypeId))]
        public ExpenseType ExpenseType { get; set; }

        #endregion
    }
}
