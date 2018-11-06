using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

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

        [Display(Name = "Cancêr")]
        public string CancerType { get; set; }

        [Display(Name = "Remédio")]
        public string Medicine { get; set; }

        [Display(Name = "Médico")]
        public string Doctor { get; set; }

        [Display(Name = "Local de Tratamento")]
        public string TreatmentPlace { get; set; }

        #region Selects

        public List<SelectListItem> CancerTypes { get; set; }

        public List<SelectListItem> Medicines { get; set; }

        public List<SelectListItem> Doctors { get; set; }

        public List<SelectListItem> TreatmentPlaces { get; set; }

        #endregion
    }
}
