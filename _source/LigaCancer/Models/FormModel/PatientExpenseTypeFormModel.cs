// <copyright file="PatientExpenseTypeFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class PatientExpenseTypeFormModel
    {
        [Display(Name = "Despesa")]
        [Required(ErrorMessage = "Este campo é obrigátorio!")]
        public string ExpenseType { get; set; }

        public List<SelectListItem> ExpenseTypes { get; set; }

        [Display(Name = "Valor")]
        [Required(ErrorMessage = "Este campo é obrigátorio!")]
        public string Value { get; set; }
    }
}
