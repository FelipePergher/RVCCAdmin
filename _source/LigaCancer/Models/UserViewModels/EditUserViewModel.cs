using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.UserViewModels
{
    public class EditUserViewModel
    {
        public EditUserViewModel()
        {
            ApplicationRoles = new List<SelectListItem>();
        }

        public string UserId { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string LastName { get; set; }

        [EmailAddress(ErrorMessage = "Insira um email válido!"), Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Email { get; set; }

        public List<SelectListItem> ApplicationRoles { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Role { get; set; }
    }
}
