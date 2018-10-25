using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Code
{
    public class Globals
    {
       
        public enum Sex
        {
            [Display(Name = "Não especificado")]
            NotSpecified,
            [Display(Name = "Homem")]
            Male,
            [Display(Name = "Mulher")]
            Female
        }

        public enum CivilState
        {
            [Display(Name = "Solteiro(a)")]
            Single,
            [Display(Name = "Casado(a)")]
            Married,
            [Display(Name = "Separado(a)")]
            Separate,
            [Display(Name = "Divorciado(a)")]
            Divorced,
            [Display(Name = "Viúvo(a)")]
            Widowed,
        }

        public enum PhoneType
        {
            [Display(Name = "Fixo")]
            landline,
            [Display(Name = "Celular")]
            cellphone
        }

        public enum ArchiveCategorie
        {
            [Display(Name = "Pessoal")]
            Personal,
            [Display(Name = "Médico")]
            Medical,
            [Display(Name = "Outros")]
            Other
        }

        public enum ResidenceType
        {
            [Display(Name = "Própria")]
            Owner,
            [Display(Name = "Cedida")]
            ceded,
            [Display(Name = "Alugada")]
            leased,
            [Display(Name = "Outros")]
            Other
        }
    }
}
