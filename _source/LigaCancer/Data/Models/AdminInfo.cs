// <copyright file="AdminInfo.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models
{
    public class AdminInfo : RegisterData
    {
        [Key]
        public int AdminInfoId { get; set; }

        public double MinSalary { get; set; }
    }
}