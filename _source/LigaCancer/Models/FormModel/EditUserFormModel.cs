using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.UserViewModels
{
    public class EditUserFormModel
    {
        public EditUserFormModel()
        {
            ApplicationRoles = new List<SelectListItem>();
        }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Insira um email válido!")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsEmailUsed", "User", AdditionalFields = "UserId", ErrorMessage = "Email já utilizado!")]
        public string Email { get; set; }

        public List<SelectListItem> ApplicationRoles { get; set; }

        [Display(Name = "Regra")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Role { get; set; }
    }
}
