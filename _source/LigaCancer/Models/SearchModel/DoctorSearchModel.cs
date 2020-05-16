// <copyright file="DoctorSearchModel.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class DoctorSearchModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "CRM")]
        public string CRM { get; set; }
    }
}
