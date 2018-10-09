using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.MedicalViewModels
{
    public class CancerTypeViewModel
    {
        public int CancerTypeId { get; set; }

        [Display(Name = "Nome"), Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsNameExist", "CancerType", AdditionalFields = "CancerTypeId", ErrorMessage = "Tipo de câncer já registrado!")]
        public string Name { get; set; }
    }
}
