using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.SearchViewModels
{
    public class PatientSearchViewModel
    {
        public PatientSearchViewModel()
        {
            CancerTypes = new List<string>();
            Doctors = new List<string>();
            TreatmentPlaces = new List<string>();
            Medicines = new List<string>();
        }

        [Display(Name = "Estado civil")]
        public string CivilState { get; set; } = "-1";

        [Display(Name = "Gênero")]
        public string Sex { get; set; } = "-1";

        [Display(Name = "Somente Óbitos")]
        public bool Death { get; set; }

        [Display(Name = "Somente Alta")]
        public bool Discharge { get; set; }

        [Display(Name = "Grupo de convivência")]
        public bool FamiliarityGroup { get; set; }

        [Display(Name = "Cancêres")]
        public List<string> CancerTypes { get; set; }

        [Display(Name = "Remédios")]
        public List<string> Medicines { get; set; }

        [Display(Name = "Médicos")]
        public List<string> Doctors { get; set; }

        [Display(Name = "Local de Tratamentos")]
        public List<string> TreatmentPlaces { get; set; }

        #region Selects

        public List<SelectListItem> SelectCancerTypes { get; set; }

        public List<SelectListItem> SelectMedicines { get; set; }

        public List<SelectListItem> SelectDoctors { get; set; }

        public List<SelectListItem> SelectTreatmentPlaces { get; set; }

        #endregion
    }
}
