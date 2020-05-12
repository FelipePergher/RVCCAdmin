// <copyright file="DateRangeAttribute.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Business
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "A data deve ser anterior a hoje!";

        public DateRangeAttribute()
            : base(DefaultErrorMessage)
        {
        }

        public override bool IsValid(object value)
        {
            return value == null || !DateTime.TryParse(value.ToString(), out DateTime dateTime) ||
                   DateTime.Now.Date > dateTime.Date;
        }
    }
}
