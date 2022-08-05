// <copyright file="ExpenseTypeSearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class ExpenseTypeSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
    }
}
