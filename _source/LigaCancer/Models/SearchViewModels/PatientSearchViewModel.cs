using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.SearchViewModels
{
    public class PatientSearchViewModel
    {
        [Display(Name = "Estado civil")]
        public string CivilState { get; set; } = "-1";

        [Display(Name = "Gênero")]
        public string Sex { get; set; } = "-1";

        [Display(Name = "Somente Óbitos")]
        public bool Death { get; set; }

        [Display(Name = "Somente Alta")]
        public bool Discharge { get; set; }
    }
}
