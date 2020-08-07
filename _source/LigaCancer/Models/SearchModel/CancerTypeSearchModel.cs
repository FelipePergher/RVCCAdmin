// <copyright file="CancerTypeSearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class CancerTypeSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
    }
}
