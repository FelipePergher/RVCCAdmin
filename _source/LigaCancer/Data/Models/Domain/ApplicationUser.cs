// <copyright file="ApplicationUser.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Identity;
using System;

namespace RVCC.Data.Models.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public DateTime RegisterTime { get; set; }

        public string CreatedBy { get; set; }
    }
}
