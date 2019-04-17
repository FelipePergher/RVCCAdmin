using Microsoft.AspNetCore.Identity;
using System;

namespace LigaCancer.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime RegisterDate { get; set; }

        public string CreatedBy { get; set; }

        public bool IsDeleted { get; set; }

        public DateTime DeletedDate { get; set; }
    }
}
