// <copyright file="MedicineSearchModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class MedicineSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }
    }
}
