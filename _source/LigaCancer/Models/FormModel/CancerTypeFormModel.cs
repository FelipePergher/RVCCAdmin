using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class CancerTypeFormModel
    {
        public CancerTypeFormModel() { }

        public CancerTypeFormModel(string name, int cancerTypeId)
        {
            Name = name;
            CancerTypeId = cancerTypeId;
        }

        [HiddenInput]
        public int CancerTypeId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsNameExist", "CancerTypeApi", AdditionalFields = "CancerTypeId", ErrorMessage = "Tipo de câncer já registrado!")]
        public string Name { get; set; }
    }
}
