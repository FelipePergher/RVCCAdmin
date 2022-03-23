// <copyright file="AuditStay.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditStay : IAudit, IStay
    {
        [Key]
        public int AuditStayId { get; set; }

        #region IStay

        public int StayId { get; set; }

        public int? PatientId { get; set; }

        public string PatientName { get; set; }

        public string City { get; set; }

        public DateTime StayDateTime { get; set; }

        public string Note { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}
