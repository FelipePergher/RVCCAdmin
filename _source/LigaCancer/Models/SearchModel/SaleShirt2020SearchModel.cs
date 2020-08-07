// <copyright file="SaleShirt2020SearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.Rendering;
using RVCC.Business;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class SaleShirt2020SearchModel
    {
        [Display(Name = "Código")]
        public string Code { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Estados")]
        public List<Enums.Status> States { get; set; }

        public List<SelectListItem> StatesList { get; set; }
    }
}
