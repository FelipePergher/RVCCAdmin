using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class DoctorFormModel
    {
        [HiddenInput]
        public int DoctorId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Name { get; set; }

        [Remote("IsCrmExist", "Doctor", AdditionalFields = "DoctorId", ErrorMessage = "CRM já registrado!", HttpMethod = "GET")]
        public string CRM { get; set; }
    }
}
