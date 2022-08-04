// <copyright file="IVisitor.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface IVisitor
    {
        public int VisitorId { get; set; }

        public string Name { get; set; }

        public string CPF { get; set; }

        public string Phone { get; set; }
    }
}