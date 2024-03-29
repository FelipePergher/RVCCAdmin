﻿// <copyright file="EditUserFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using RVCC.Business;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class EditUserFormModel
    {
        public EditUserFormModel()
        {
        }

        public EditUserFormModel(string userId, string name)
        {
            UserId = userId;
            Name = name;
        }

        [HiddenInput]
        public string UserId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Name { get; set; }

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
