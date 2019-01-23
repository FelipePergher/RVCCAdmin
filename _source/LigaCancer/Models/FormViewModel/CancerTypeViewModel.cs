using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormViewModel
{
    public class CancerTypeViewModel
    {
        public int CancerTypeId { get; set; }

        [Display(Name = "Nome"), Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsNameExist", "CancerType", AdditionalFields = "CancerTypeId", ErrorMessage = "Tipo de câncer já registrado!")]
        public string Name { get; set; }
    }
}
