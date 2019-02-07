using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.SearchViewModel
{
    public class DoctorSearchViewModel
    {
        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "CRM")]
        public string CRM { get; set; }
    }
}
