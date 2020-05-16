// <copyright file="PhoneFormModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using RVCC.Business;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class PhoneFormModel
    {
        [Display(Name = "Número")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        [StringLength(15, MinimumLength = 14, ErrorMessage = "Telefone inválido")]
        public string Number { get; set; }

        [Display(Name = "Tipo de telefone")]
        public Globals.PhoneType? PhoneType { get; set; }

        [Display(Name = "Observações")]
        [StringLength(1000, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string ObservationNote { get; set; }
    }
}
