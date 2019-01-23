using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormViewModel
{
    public class DoctorFormViewModel
    {
        public int DoctorId { get; set; }

        [Display(Name = "Nome"), Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Name { get; set; }

        [Remote("IsCRMExist", "Doctor", AdditionalFields = "DoctorId", ErrorMessage = "CRM já registrado!")]
        public string CRM { get; set; }
    }
}
