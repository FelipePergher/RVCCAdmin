// <copyright file="AuditSetting.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditSetting : IAudit, ISetting
    {
        [Key]
        public int AuditSettingId { get; set; }

        #region ISetting

        public int SettingId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}