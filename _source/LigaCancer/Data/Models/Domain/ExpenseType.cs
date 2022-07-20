// <copyright file="ExpenseType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static RVCC.Business.Enums;

namespace RVCC.Data.Models.Domain
{
    public class ExpenseType : RegisterData, IExpenseType
    {
        public ExpenseType()
        {
        }

        public ExpenseType(string name, ExpenseTypeFrequency expenseTypeFrequency)
        {
            Name = name;
            ExpenseTypeFrequency = expenseTypeFrequency;
        }

        #region IExpenseType

        [Key]
        public int ExpenseTypeId { get; set; }

        public string Name { get; set; }

        public ExpenseTypeFrequency ExpenseTypeFrequency { get; set; }

        #endregion

        #region Relations

        public ICollection<PatientExpenseType> PatientExpenseTypes { get; set; }

        #endregion
    }
}