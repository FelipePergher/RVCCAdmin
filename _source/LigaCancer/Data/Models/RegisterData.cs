using System;

namespace RVCC.Data.Models
{
    public abstract class RegisterData
    {
        public RegisterData()
        {
            RegisterTime = DateTime.Now;
        }

        public DateTime RegisterTime { get; set; }

        public string CreatedBy { get; set; }

        public DateTime UpdatedTime { get; set; }

        public string UpdatedBy { get; set; }

    }
}
