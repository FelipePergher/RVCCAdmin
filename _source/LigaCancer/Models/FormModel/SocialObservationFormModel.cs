// <copyright file="SocialObservationFormModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class SocialObservationFormModel
    {
        [Display(Name = "Observações")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [StringLength(5000, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Observations { get; set; }
    }
}
