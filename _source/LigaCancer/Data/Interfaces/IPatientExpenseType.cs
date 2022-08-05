// <copyright file="IPatientExpenseType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface IPatientExpenseType
    {
        public int PatientExpenseTypeId { get; set; }

        public int PatientId { get; set; }

        public int ExpenseTypeId { get; set; }

        public double Value { get; set; }
    }
}