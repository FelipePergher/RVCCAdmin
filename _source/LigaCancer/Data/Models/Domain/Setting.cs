// <copyright file="Setting.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Domain
{
    public class Setting : ISetting
    {
        public Setting()
        {
        }

        public Setting(string key, string value)
        {
            Key = key;
            Value = value;
        }

        #region ISetting

        [Key]
        public int SettingId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        #endregion

        public decimal GetValueAsDecimal()
        {
            return decimal.TryParse(Value, out decimal decimalValue) ? decimalValue : 0;
        }

        public double GetValueAsDouble()
        {
            return double.TryParse(Value, out double doubleValue) ? doubleValue : 0;
        }

        public int GetValueAsInt()
        {
            return int.TryParse(Value, out int intValue) ? intValue : 0;
        }
    }
}