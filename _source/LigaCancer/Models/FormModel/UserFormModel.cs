﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class UserFormModel
    {
        public UserFormModel() { }

        public UserFormModel(string userId)
        {
            UserId = userId;
        }

        [HiddenInput]
        public string UserId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Name { get; set; }

        [EmailAddress(ErrorMessage = "Insira um email válido!")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsEmailUsed", "UserApi", AdditionalFields = "UserId", ErrorMessage = "Email já utilizado!")]
        public string Email { get; set; }

        [Display(Name = "Senha")]
        [DataType(DataType.Password)]
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
                    new SelectListItem("Administrador", "Admin"),
                    new SelectListItem("Usuário", "User")
                };

        [Display(Name = "Regra")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Role { get; set; }
    }
}