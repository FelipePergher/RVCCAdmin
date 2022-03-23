// <copyright file="IPresence.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;

namespace RVCC.Data.Interfaces
{
    public interface IPresence
    {
        public int PresenceId { get; set; }

        public int PatientId { get; set; }

        public string Name { get; set; }

        public DateTime PresenceDateTime { get; set; }
    }
}