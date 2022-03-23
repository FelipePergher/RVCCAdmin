﻿// <copyright file="AuditDoctor.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditDoctor : IAudit, IDoctor
    {
        [Key]
        public int AuditDoctorId { get; set; }

        #region IDoctor

        public int DoctorId { get; set; }

        public string Name { get; set; }

        public string CRM { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}