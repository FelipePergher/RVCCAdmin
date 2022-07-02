// <copyright file="AuditPhone.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditPhone : IAudit, IPhone
    {
        [Key]
        public int AuditPhoneId { get; set; }

        #region IPhone

        public int PhoneId { get; set; }

        public string Number { get; set; }

        public Enums.PhoneType? PhoneType { get; set; }

        public string ObservationNote { get; set; }

        public int PatientId { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}