// <copyright file="ICancerType.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface ICancerType
    {
        public int CancerTypeId { get; set; }

        public string Name { get; set; }
    }
}