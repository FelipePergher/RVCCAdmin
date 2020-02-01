using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RVCC.Business;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class EditUserFormModel
    {
        public EditUserFormModel() { }

        public EditUserFormModel(string userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        [HiddenInput]
        public string UserId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Name { get; set; }

        public List<SelectListItem> ApplicationRoles => new List<SelectListItem>
                {
                    new SelectListItem("Administrador", Roles.Admin),
                    new SelectListItem("Usuário", Roles.User)
                };

        [Display(Name = "Regra")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Role { get; set; }
    }
}
