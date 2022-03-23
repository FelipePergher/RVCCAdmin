// <copyright file="IDoctor.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface IDoctor
    {
        public int DoctorId { get; set; }

        public string Name { get; set; }

        public string CRM { get; set; }
    }
}