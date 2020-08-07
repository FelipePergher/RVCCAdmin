// <copyright file="CancerTypeFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class CancerTypeFormModel
    {
        public CancerTypeFormModel()
        {
        }

        public CancerTypeFormModel(string name, int cancerTypeId)
        {
            Name = name;
            CancerTypeId = cancerTypeId;
        }

        [HiddenInput]
        public int CancerTypeId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsNameExist", "CancerTypeApi", AdditionalFields = "CancerTypeId", ErrorMessage = "Tipo de câncer já registrado!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }
    }
}
