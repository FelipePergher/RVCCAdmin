// <copyright file="ISetting.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface ISetting
    {
        public int SettingId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }
    }
}