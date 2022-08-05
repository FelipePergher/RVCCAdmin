// <copyright file="AttendanceTypeFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class AttendanceTypeFormModel
    {
        public AttendanceTypeFormModel()
        {
        }

        public AttendanceTypeFormModel(string name, int attendanceTypeId)
        {
            Name = name;
            AttendanceTypeId = attendanceTypeId;
        }

        [HiddenInput]
        public int AttendanceTypeId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsNameExist", "AttendanceTypeApi", AdditionalFields = "AttendanceTypeId", ErrorMessage = "Tipo de atendimento já registrado!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }
    }
}
