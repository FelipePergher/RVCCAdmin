// <copyright file="UserFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RVCC.Business;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class UserFormModel
    {
        public UserFormModel()
        {
        }

        public UserFormModel(string userId)
        {
            UserId = userId;
        }

        [HiddenInput]
        public string UserId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Insira um email válido!")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsEmailUsed", "UserApi", AdditionalFields = "UserId", ErrorMessage = "Email já utilizado!")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
        [StringLength(100, ErrorMessage = "A {0} deve ter ao menos {2} e no máximo {1} caracteres.", MinimumLength = 8)]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*\d).{8,}$", ErrorMessage = "A senha deve conter letras, numeros e minimo de 8 caracteres")]
        public string Password { get; set; }

        [Display(Name = "Confirmação de senha")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Compare("Password", ErrorMessage = "Confirmação de senha não confere")]
        public string ConfirmPassword { get; set; }

        public List<SelectListItem> ApplicationRoles => new List<SelectListItem>
        {
            new SelectListItem("Secretária", Roles.Secretary),
            new SelectListItem("Assistente Social", Roles.SocialAssistance),
            new SelectListItem("Administrador", Roles.Admin)
        };

        [Display(Name = "Regra")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Role { get; set; }
    }
}
