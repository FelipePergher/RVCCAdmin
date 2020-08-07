// <copyright file="TreatmentPlaceFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class TreatmentPlaceFormModel
    {
        public TreatmentPlaceFormModel()
        {
        }

        public TreatmentPlaceFormModel(string city, int treatmentPlaceId)
        {
            City = city;
            TreatmentPlaceId = treatmentPlaceId;
        }

        [HiddenInput]
        public int TreatmentPlaceId { get; set; }

        [Display(Name = "Cidade")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsCityExist", "TreatmentPlaceApi", AdditionalFields = "TreatmentPlaceId", ErrorMessage = "Cidade já registrada!")]
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string City { get; set; }
    }
}
