// <copyright file="FamilyMemberFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class FamilyMemberFormModel
    {
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigátorio!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }

        [Display(Name = "Parentesco")]
        [Required(ErrorMessage = "Este campo é obrigátorio!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Kinship { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DateRange]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Gênero")]
        public Enums.Sex Sex { get; set; }

        [Display(Name = "Renda mensal")]
        public string MonthlyIncome { get; set; }

        [Display(Name = "Renda mensal (Sal min)")]
        public string MonthlyIncomeMinSalary { get; set; }
    }
}
