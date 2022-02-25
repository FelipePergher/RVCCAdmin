// <copyright file="BirthdaySearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel;

public class BirthdaySearchModel
{
    public BirthdaySearchModel()
    {
        Month = DateTime.Now.AddDays(-30).ToString("MMMM");
    }

    [Display(Name = "Nome")]
    public string Name { get; set; }

    [Display(Name = "Mês")]
    [DisplayFormat(DataFormatString = @"{0:MM}", ApplyFormatInEditMode = true)]
    [DataType(DataType.Date)]
    public string Month { get; set; }
}