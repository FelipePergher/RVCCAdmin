using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.MedicalViewModels
{
    public class PatientViewModel
    {
        public int PatientId { get; set; }

        [Display(Name = "Nome"), Required(ErrorMessage = "Este campo é obrigatório!"),
            Remote("IsNameExist", "Patient", AdditionalFields = "PatientId", ErrorMessage = "Remédio já registrado!")]
        public string Name { get; set; }
    }
}
