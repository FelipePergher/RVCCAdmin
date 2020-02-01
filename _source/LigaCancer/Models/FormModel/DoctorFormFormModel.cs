using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class DoctorFormModel
    {
        public DoctorFormModel() { }

        public DoctorFormModel(string name, string crm, int doctorId)
        {
            Name = name;
            CRM = crm;
            DoctorId = doctorId;
        }

        [HiddenInput]
        public int DoctorId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Name { get; set; }

        [Remote("IsCrmExist", "DoctorApi", AdditionalFields = "DoctorId", ErrorMessage = "CRM já registrado!")]
        public string CRM { get; set; }
    }
}
