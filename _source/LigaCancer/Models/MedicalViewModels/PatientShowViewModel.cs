using LigaCancer.Data.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static LigaCancer.Code.Globals;

namespace LigaCancer.Models.MedicalViewModels
{
    public class PatientShowViewModel
    {
        public int PatientId { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        public string RG { get; set; }

        public string CPF { get; set; }

        [Display(Name = "Grupo de Convivência")]
        public bool FamiliarityGroup { get; set; }

        [Display(Name = "Sexo")]
        public Sex Sex { get; set; }

        [Display(Name = "Estado Civil")]
        public CivilState CivilState { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Profissão")]
        public string Profession { get; set; }

        public NaturalityViewModel Naturality { get; set; }

        public PatientInformationViewModel PatientInformation { get; set; }

        public ICollection<Phone> Phones { get; set; }

        public List<AddressViewModel> Addresses { get; set; }

        public FamilyViewModel Family { get; set; }

        public AttachmentsViewModel Attachments { get; set; }
    }
}
