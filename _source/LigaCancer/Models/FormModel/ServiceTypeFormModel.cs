// <copyright file="ServiceTypeFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class ServiceTypeFormModel
    {
        public ServiceTypeFormModel()
        {
        }

        public ServiceTypeFormModel(string name, int serviceTypeId)
        {
            Name = name;
            ServiceTypeId = serviceTypeId;
        }

        [HiddenInput]
        public int ServiceTypeId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsNameExist", "ServiceTypeApi", AdditionalFields = "ServiceTypeId", ErrorMessage = "Tipo de serviço já registrado!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }
    }
}
