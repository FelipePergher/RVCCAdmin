// <copyright file="PatientExpenseTypeSearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class PatientExpenseTypeSearchModel
    {
        public string PatientId { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Benefício")]
        public string ExpenseType { get; set; }
    }
}
