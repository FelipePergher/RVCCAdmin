// <copyright file="TreatmentPlaceSearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class TreatmentPlaceSearchModel
    {
        [Display(Name = "Local de Tratamento")]
        public string City { get; set; }
    }
}
