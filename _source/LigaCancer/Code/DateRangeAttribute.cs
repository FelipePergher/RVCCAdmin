using System;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Code
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "A data deve ser anterior a hoje!";

        public DateRangeAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            if (value == null || !(value is DateTime))
            {
                return true;
            }

            DateTime dateValue = (DateTime)value;
            return DateTime.Now.Date > dateValue.Date;
        }
    }
}
