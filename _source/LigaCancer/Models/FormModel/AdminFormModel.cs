// <copyright file="AdminFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class AdminFormModel
    {
        [Display(Name = "Salário mínimo")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string MinSalary { get; set; }
    }
}
