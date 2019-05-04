using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class MedicineFormModel
    {
        [Display(Name = "Nome"), Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsNameExist", "Medicine", AdditionalFields = "MedicineId", ErrorMessage = "Remédio já registrado!")]
        public string Name { get; set; }
    }
}
