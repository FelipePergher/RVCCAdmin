// <copyright file="TreatmentTypeFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class TreatmentTypeFormModel
    {
        public TreatmentTypeFormModel()
        {
        }

        public TreatmentTypeFormModel(string name, string note, int treatmentTypeId)
        {
            Name = name;
            Note = note;
            TreatmentTypeId = treatmentTypeId;
        }

        [HiddenInput]
        public int TreatmentTypeId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsNameExist", "TreatmentTypeApi", AdditionalFields = "TreatmentTypeId", ErrorMessage = "Tipo de tratamenot já registrado!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }

        public string Note { get; set; }
    }
}
