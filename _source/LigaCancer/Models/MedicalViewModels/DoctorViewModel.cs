using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.MedicalViewModels
{
    public class DoctorViewModel
    {
        public int DoctorId { get; set; }

        [Display(Name = "Nome"), Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Name { get; set; }

        [Remote("IsCRMExist", "Doctor", AdditionalFields = "DoctorId", ErrorMessage = "CRM já registrado!")]
        public string CRM { get; set; }
    }
}
