using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Business
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private const string DefaultErrorMessage = "A data deve ser anterior a hoje!";

        public DateRangeAttribute() : base(DefaultErrorMessage) { }

        public override bool IsValid(object value)
        {
            if (value == null || !DateTime.TryParse(value.ToString(), out DateTime dateTime))
            {
                return true;
            }

            return DateTime.Now.Date > dateTime.Date;
        }
    }
}
