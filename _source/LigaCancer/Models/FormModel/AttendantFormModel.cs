// <copyright file="AttendantFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class AttendantFormModel
    {
        public AttendantFormModel()
        {
        }

        public AttendantFormModel(string name, string cpf, string phone, int attendantId, string note)
        {
            Name = name;
            CPF = cpf;
            Phone = phone;
            AttendantId = attendantId;
            Note = note;
        }

        [HiddenInput]
        public int AttendantId { get; set; }

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

        [Display(Name = "Nota")]
        [StringLength(500, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Note { get; set; }
    }
}
