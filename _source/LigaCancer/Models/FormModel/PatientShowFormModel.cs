using LigaCancer.Code;
using LigaCancer.Data.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class PatientShowFormModel
    {
        public int PatientId { get; set; }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        public string RG { get; set; }

        public string CPF { get; set; }

        [Display(Name = "Grupo de Convivência")]
        public bool FamiliarityGroup { get; set; }

        [Display(Name = "Gênero")]
        public Globals.Sex Sex { get; set; }

        [Display(Name = "Estado Civil")]
        public Globals.CivilState CivilState { get; set; }

        [Display(Name = "Data de Nascimento")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Profissão")]
        public string Profession { get; set; }

        public NaturalityFormModel Naturality { get; set; }

        public PatientInformationFormModel PatientInformation { get; set; }

        public Family Family { get; set; }

        public IEnumerable<FileAttachment> FileAttachments { get; set; }
    }
}
