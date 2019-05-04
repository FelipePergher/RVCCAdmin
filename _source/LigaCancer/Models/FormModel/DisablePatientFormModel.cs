using LigaCancer.Code;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class ArchivePatientFormModel
    {
        [Display(Name = "Motivo")]
        public Globals.ArchivePatientType ArchivePatientType { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [Display(Name = "Data")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateTime { get; set; }
    }
}
