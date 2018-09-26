using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace LigaCancer.Data.Models
{
    public abstract class RegisterData
    {
        public DateTime RegisterDate { get; set; }

        public ApplicationUser UserCreated { get; set; }

        public string IPAddressCreated { get; set; }


        public DateTime LastUpdatedDate { get; set; }

        public ApplicationUser LastUserUpdate { get; set; }

        public string LastIPAddressUpdated { get; set; }
    }
}
