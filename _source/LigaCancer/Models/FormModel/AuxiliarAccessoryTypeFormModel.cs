// <copyright file="AuxiliarAccessoryTypeFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class AuxiliarAccessoryTypeFormModel
    {
        public AuxiliarAccessoryTypeFormModel()
        {
        }

        public AuxiliarAccessoryTypeFormModel(string name, int auxiliarAccessoryTypeId)
        {
            Name = name;
            AuxiliarAccessoryTypeId = auxiliarAccessoryTypeId;
        }

        [HiddenInput]
        public int AuxiliarAccessoryTypeId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsNameExist", "AuxiliarAccessoryTypeApi", AdditionalFields = "AuxiliarAccessoryTypeId", ErrorMessage = "Tipo de Acessório Auxiliar já registrado!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }
    }
}
