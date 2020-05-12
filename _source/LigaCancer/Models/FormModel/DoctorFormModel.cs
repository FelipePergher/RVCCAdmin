// <copyright file="DoctorFormModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class DoctorFormModel
    {
        public DoctorFormModel()
        {
        }

        public DoctorFormModel(string name, string crm, int doctorId)
        {
            Name = name;
            CRM = crm;
            DoctorId = doctorId;
        }

        [HiddenInput]
        public int DoctorId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }

        [Remote("IsCrmExist", "DoctorApi", AdditionalFields = "DoctorId", ErrorMessage = "CRM já registrado!")]
        [StringLength(50, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string CRM { get; set; }
    }
}
