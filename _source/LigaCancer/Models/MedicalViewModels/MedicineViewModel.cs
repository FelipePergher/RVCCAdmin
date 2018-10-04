using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.MedicalViewModels
{
    public class MedicineViewModel
    {
        public int MedicineId { get; set; }

        [Display(Name = "Nome"), Required(ErrorMessage = "Este campo é obrigatório!"),
            Remote("IsNameExist", "Medicine", AdditionalFields = "MedicineId", ErrorMessage = "Remédio já registrado!")]
        public string Name { get; set; }
    }
}
