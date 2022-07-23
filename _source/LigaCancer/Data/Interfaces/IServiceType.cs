// <copyright file="IServiceType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface IServiceType
    {
        public int ServiceTypeId { get; set; }

        public string Name { get; set; }
    }
}