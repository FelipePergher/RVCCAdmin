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
            [Display(Name = "Homem")]
            Male = 0,
            [Display(Name = "Mulher")]
            Female = 1,
            [Display(Name = "Não especificado")]
            NotSpecified = 2,

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
    }
}
