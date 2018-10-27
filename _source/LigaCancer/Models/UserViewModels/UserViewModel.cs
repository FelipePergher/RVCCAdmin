using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.UserViewModels
{
    public class UserViewModel
    {
        public UserViewModel()
        {
            ApplicationRoles = new List<SelectListItem>();
        }

        public string UserId { get; set; }
       
        [Display(Name = "Nome"), Required(ErrorMessage = "Este campo é obrigatório!")]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome"), Required(ErrorMessage = "Este campo é obrigatório!")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Insira um email válido!"), 
            Required(ErrorMessage = "Este campo é obrigatório!"),
            Remote("IsEmailUsed", "User", AdditionalFields = "UserId", ErrorMessage = "Email já utilizado!")]
        public string Email { get; set; }

        [Display(Name = "Senha"), DataType(DataType.Password), Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Password { get; set; }

        [Display(Name = "Confirmação de senha"), 
            DataType(DataType.Password), 
            Required(ErrorMessage = "Este campo é obrigatório!"),
            Compare("Password", ErrorMessage = "Confirmação de senha não confere")]
        public string ConfirmPassword { get; set; }

        public List<SelectListItem> ApplicationRoles { get; set; }

        [Display(Name = "Roles"), Required(ErrorMessage = "Este campo é obrigatório!")]
        public string RoleId { get; set; }
    }
}
