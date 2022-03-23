// <copyright file="Presence.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using RVCC.Data.Models.Audit;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.Domain
{
    public class Presence : RegisterData, IPresence
    {
        #region IPresence

        [Key]
        public int PresenceId { get; set; }

        public int PatientId { get; set; }

        public string Name { get; set; }

        public DateTime PresenceDateTime { get; set; }

        #endregion

        #region Relations

        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        #endregion
    }
}
