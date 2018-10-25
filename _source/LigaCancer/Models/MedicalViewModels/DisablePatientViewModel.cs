using LigaCancer.Code;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.MedicalViewModels
{
    public class DisablePatientViewModel
    {
        [Display(Name = "Motivo")]
        public Globals.DisablePatientType DisablePatientType { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório."), Display(Name = "Data"), DataType(DataType.Date)]
        public DateTime DateTime { get; set; }
    }
}
