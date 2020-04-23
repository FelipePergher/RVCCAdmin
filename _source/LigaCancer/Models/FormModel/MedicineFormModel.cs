using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
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
        [StringLength(200, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }
    }
}
