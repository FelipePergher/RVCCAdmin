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

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Sobrenome")]
        public string Surname { get; set; }

        [Display(Name = "RG")]
        public string Rg { get; set; }

        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Display(Name = "Estado civil")]
        public string CivilState { get; set; }

        [Display(Name = "Gênero")]
        public string Sex { get; set; }

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

        [Display(Name = "Locais de Tratamentos")]
        public List<string> TreatmentPlaces { get; set; }

        public List<SelectListItem> FamiliarityGroups => new List<SelectListItem>
            {
                new SelectListItem("", ""),
                new SelectListItem("Participa", "true"),
                new SelectListItem("Não Participa", "false")
            };
    }
}
