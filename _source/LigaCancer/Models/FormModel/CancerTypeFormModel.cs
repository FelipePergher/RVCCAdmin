using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class CancerTypeFormModel
    {
        [HiddenInput]
        public int CancerTypeId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsNameExist", "CancerType", AdditionalFields = "CancerTypeId", ErrorMessage = "Tipo de câncer já registrado!")]
        public string Name { get; set; }
    }
}
