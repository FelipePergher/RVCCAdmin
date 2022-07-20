// <copyright file="IExpenseType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using static RVCC.Business.Enums;

namespace RVCC.Data.Interfaces
{
    public interface IExpenseType
    {
        public int ExpenseTypeId { get; set; }

        public string Name { get; set; }

        public ExpenseTypeFrequency ExpenseTypeFrequency { get; set; }
    }
}