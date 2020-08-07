// <copyright file="DoctorSearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
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
