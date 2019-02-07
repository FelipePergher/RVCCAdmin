using LigaCancer.Code;
using System;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class DisablePatientFormModel
    {
        [Display(Name = "Motivo")]
        public Globals.DisablePatientType DisablePatientType { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório."), Display(Name = "Data"), DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
    }
}
