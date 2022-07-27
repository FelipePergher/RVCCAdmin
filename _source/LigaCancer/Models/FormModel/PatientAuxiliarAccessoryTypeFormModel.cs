// <copyright file="PatientAuxiliarAccessoryTypeFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.Rendering;
using RVCC.Business;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using static RVCC.Business.Enums;

namespace RVCC.Models.FormModel
{
    public class PatientAuxiliarAccessoryTypeFormModel
    {
        [Display(Name = "Acessório Auxiliar")]
        [Required(ErrorMessage = "Este campo é obrigátorio!")]
        public string AuxiliarAccessoryType { get; set; }

        public List<SelectListItem> AuxiliarAccessoryTypes { get; set; }

        [Display(Name = "Notas")]
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Note { get; set; }

        [Display(Name = "Data estimada para parar utilização")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string DuoDate { get; set; }

        [Display(Name = "Tipo de uso")]
        public AuxiliarAccessoryTypeTime AuxiliarAccessoryTypeTime { get; set; }
    }
}
