// <copyright file="RegisterData.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;

namespace RVCC.Data.Models.Audit
{
    public abstract class RegisterData
    {
        public DateTime RegisterTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string UpdatedBy { get; set; }
    }
}
