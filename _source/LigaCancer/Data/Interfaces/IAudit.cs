// <copyright file="IAudit.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;

namespace RVCC.Data.Interfaces
{
    public interface IAudit
    {
        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }
    }
}
