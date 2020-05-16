// <copyright file="TreatmentPlaceSearchModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class TreatmentPlaceSearchModel
    {
        [Display(Name = "Cidade")]
        public string City { get; set; }
    }
}
