// <copyright file="ExpenseTypeFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using static RVCC.Business.Enums;

namespace RVCC.Models.FormModel
{
    public class ExpenseTypeFormModel
    {
        public ExpenseTypeFormModel()
        {
        }

        public ExpenseTypeFormModel(string name, ExpenseTypeFrequency expenseTypeFrequency, int expenseTypeId)
        {
            Name = name;
            ExpenseTypeId = expenseTypeId;
            ExpenseTypeFrequency = expenseTypeFrequency;
        }

        [HiddenInput]
        public int ExpenseTypeId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsNameExist", "ExpenseTypeApi", AdditionalFields = "ExpenseTypeId", ErrorMessage = "Tipo de despesa já registrado!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }

        [Display(Name = "Frequencia da Despesa")]
        public ExpenseTypeFrequency ExpenseTypeFrequency { get; set; }
    }
}
