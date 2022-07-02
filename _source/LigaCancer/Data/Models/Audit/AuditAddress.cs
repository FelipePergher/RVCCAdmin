// <copyright file="AuditAddress.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Business;
using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditAddress : IAudit, IAddress
    {
        [Key]
        public int AuditAddressId { get; set; }

        #region IAudit

        public int AddressId { get; set; }

        public string Street { get; set; }

        public string Neighborhood { get; set; }

        public string City { get; set; }

        public string HouseNumber { get; set; }

        public string Complement { get; set; }

        public string ObservationAddress { get; set; }

        public Enums.ResidenceType? ResidenceType { get; set; }

        public double MonthlyAmountResidence { get; set; }

        public int PatientId { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}