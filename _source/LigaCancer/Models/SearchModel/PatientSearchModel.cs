using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.SearchModel
{
    public class PatientSearchModel
    {
        public PatientSearchModel()
        {
            CancerTypes = new List<string>();
            Doctors = new List<string>();
            TreatmentPlaces = new List<string>();
            Medicines = new List<string>();
        }

        //Todo why use this -1?
        [Display(Name = "Estado civil")]
        public string CivilState { get; set; } = "-1";

        [Display(Name = "Gênero")]
        public string Sex { get; set; } = "-1";

        [Display(Name = "Somente paciente com óbito")]
        public bool Death { get; set; }

        [Display(Name = "Somente pacientes com alta")]
        public bool Discharge { get; set; }

        [Display(Name = "Grupo de convivência")]
        public string FamiliarityGroup { get; set; }

        [Display(Name = "Cânceres")]
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

        public List<SelectListItem> FamiliarityGroups => new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "",
                    Value = ""
                },
                new SelectListItem
                {
                    Text = "Participa",
                    Value = "true"
                },
                new SelectListItem
                {
                    Text = "Não Participa",
                    Value = "false"
                }
            };

        #endregion
    }
}
