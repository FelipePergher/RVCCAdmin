using LigaCancer.Code;
using LigaCancer.Data.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class PatientShowFormModel
    {
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
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DateRange]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Profissão")]
        public string Profession { get; set; }

        public NaturalityFormModel Naturality { get; set; }

        public PatientInformationFormModel PatientInformation { get; set; }

        public IEnumerable<FileAttachment> FileAttachments { get; set; }
    }
}
