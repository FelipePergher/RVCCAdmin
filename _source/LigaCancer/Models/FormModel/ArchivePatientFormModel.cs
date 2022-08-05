// <copyright file="ArchivePatientFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class ArchivePatientFormModel
    {
        public ArchivePatientFormModel()
        {
            DateTime = System.DateTime.Now.ToDateString();
        }

        [Display(Name = "Motivo")]
        public Enums.ArchivePatientType ArchivePatientType { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DateRange]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string DateTime { get; set; }
    }
}
