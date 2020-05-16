// <copyright file="AddressFormModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using RVCC.Business;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class AddressFormModel
    {
        [Display(Name = "Rua")]
        [Required(ErrorMessage = "Este campo é Obrigatório!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Street { get; set; }

        [Display(Name = "Bairro")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Neighborhood { get; set; }

        [Display(Name = "Cidade")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string City { get; set; }

        [Display(Name = "Número")]
        [StringLength(10, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string HouseNumber { get; set; }

        [Display(Name = "Complemento")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Complement { get; set; }

        [Display(Name = "Observação")]
        [StringLength(500, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string ObservationAddress { get; set; }

        [Display(Name = "Tipo de Residência")]
        public Globals.ResidenceType? ResidenceType { get; set; }

        [Display(Name = "Valor Mensal Residência")]
        public string MonthlyAmmountResidence { get; set; }
    }
}
