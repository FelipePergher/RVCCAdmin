using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace RVCC.Business
{
    public class Globals
    {

        [JsonConverter(typeof(StringEnumConverter))]
        public enum Sex
        {
            [Display(Name = "Não especificado"), EnumMember(Value = "Não especificado")]
            NotSpecified,
            [Display(Name = "Homem"), EnumMember(Value = "Homem")]
            Male,
            [Display(Name = "Mulher"), EnumMember(Value = "Mulher")]
            Female
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum CivilState
        {
            [Display(Name = "Solteiro(a)"), EnumMember(Value = "Solteiro(a)")]
            Single,
            [Display(Name = "Casado(a)"), EnumMember(Value = "Casado(a)")]
            Married,
            [Display(Name = "Separado(a)"), EnumMember(Value = "Separado(a)")]
            Separate,
            [Display(Name = "Divorciado(a)"), EnumMember(Value = "Divorciado(a)")]
            Divorced,
            [Display(Name = "Viúvo(a)"), EnumMember(Value = "Viúvo(a)")]
            Widowed,
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum PhoneType
        {
            [Display(Name = "Fixo"), EnumMember(Value = "Fixo")]
            landline,
            [Display(Name = "Celular"), EnumMember(Value = "Celular")]
            cellphone
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum ResidenceType
        {
            [Display(Name = "Própria"), EnumMember(Value = "Própria")]
            Owner,
            [Display(Name = "Cedida"), EnumMember(Value = "Cedida")]
            ceded,
            [Display(Name = "Alugada"), EnumMember(Value = "Alugada")]
            leased,
            [Display(Name = "Outros"), EnumMember(Value = "Outros")]
            Other
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum ArchivePatientType
        {
            [Display(Name = "Alta"), EnumMember(Value = "Alta")]
            discharge,
            [Display(Name = "Óbito"), EnumMember(Value = "Óbito")]
            death
        }

        public static string GetDisplayName(Enum enumValue)
        {
            if (enumValue == null)
            {
                return string.Empty;
            }
            else
            {
                return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First()
                            .GetCustomAttribute<DisplayAttribute>()
                            .GetName();
            }

        }

    }
}
