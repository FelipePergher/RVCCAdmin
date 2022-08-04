// <copyright file="VisitorFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class VisitorFormModel
    {
        public VisitorFormModel()
        {
        }

        public VisitorFormModel(string name, string cpf, string phone, int visitorId)
        {
            Name = name;
            CPF = cpf;
            Phone = phone;
            VisitorId = visitorId;
        }

        [HiddenInput]
        public int VisitorId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }

        [Display(Name = "CPF")]
        [StringLength(50, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string CPF { get; set; }

        [Display(Name = "Telefone")]
        [StringLength(50, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Phone { get; set; }
    }
}
