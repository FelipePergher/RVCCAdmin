// <copyright file="VisitorSearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class VisitorSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "CPF")]
        public string CPF { get; set; }

        [Display(Name = "Telefone")]
        public string Phone { get; set; }
    }
}
