using LigaCancer.Code;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class PhoneFormModel
    {
        public PhoneFormModel(){}

        public PhoneFormModel(string patientId) => PatientId = patientId;

        public PhoneFormModel(int phoneId) => PhoneId = phoneId;

        [HiddenInput]
        public int PhoneId { get; set; }

        [HiddenInput]
        public string PatientId { get; set; }

        [Display(Name = "Número")]
        [Required(ErrorMessage = "Este campo é obrigatório")]
        public string Number { get; set; }

        [Display(Name = "Tipo de telefone")]
        public Globals.PhoneType PhoneType { get; set; }

        [Display(Name = "Observações")]
        public string ObservationNote { get; set; }
    }
}
