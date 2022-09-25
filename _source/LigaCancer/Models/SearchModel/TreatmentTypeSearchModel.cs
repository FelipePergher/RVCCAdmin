// <copyright file="TreatmentTypeSearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class TreatmentTypeSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Nota")]
        public string Note { get; set; }
    }
}
