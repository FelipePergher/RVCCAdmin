using System;

namespace LigaCancer.Data.Models
{
    public abstract class RegisterData
    {
        public RegisterData()
        {
            RegisterDate = DateTime.Now;
        }

        public DateTime RegisterDate { get; set; }

        public ApplicationUser UserCreated { get; set; }


        public DateTime UpdatedDate { get; set; }

        public ApplicationUser UserUpdated { get; set; }

    }
}
