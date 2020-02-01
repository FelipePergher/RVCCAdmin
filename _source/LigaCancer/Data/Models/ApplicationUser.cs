using Microsoft.AspNetCore.Identity;
using System;

namespace RVCC.Data.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }

        public DateTime RegisterTime { get; set; }

        public string CreatedBy { get; set; }
    }
}
