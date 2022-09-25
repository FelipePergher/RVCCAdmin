// <copyright file="PatientTreatmentTypeFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class PatientTreatmentTypeFormModel
    {
        [Display(Name = "Tratamento")]
        [Required(ErrorMessage = "Este campo é obrigátorio!")]
        public string TreatmentType { get; set; }

        public List<SelectListItem> TreatmentTypes { get; set; }

        [Display(Name = "Início Tratamente")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string StartDate { get; set; }

        [Display(Name = "Término Tratamento")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string EndDate { get; set; }

        [Display(Name = "Nota")]
        [StringLength(500, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Note { get; set; }
    }
}
