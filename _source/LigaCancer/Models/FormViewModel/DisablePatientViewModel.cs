using LigaCancer.Code;
using System;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormViewModel
{
    public class DisablePatientViewModel
    {
        [Display(Name = "Motivo")]
        public Globals.DisablePatientType DisablePatientType { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório."), Display(Name = "Data"), DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
    }
}
