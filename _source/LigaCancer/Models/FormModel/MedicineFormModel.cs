using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class MedicineFormModel
    {
        public MedicineFormModel() { }

        public MedicineFormModel(string name, int medicineId)
        {
            Name = name;
            MedicineId = medicineId;
        }

        [HiddenInput]
        public int MedicineId { get; set; }

        [Display(Name = "Nome"), Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsNameExist", "MedicineApi", AdditionalFields = "MedicineId", ErrorMessage = "Remédio já registrado!")]
        public string Name { get; set; }
    }
}
