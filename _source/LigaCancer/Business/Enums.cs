// <copyright file="Enums.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace RVCC.Business
{
    public class Enums
    {
        #region Patient Enums

        [JsonConverter(typeof(StringEnumConverter))]
        public enum Sex
        {
            [Display(Name = "Não especificado")]
            [EnumMember(Value = "Não especificado")]
            NotSpecified,

            [Display(Name = "Homem")]
            [EnumMember(Value = "Homem")]
            Male,

            [Display(Name = "Mulher")]
            [EnumMember(Value = "Mulher")]
            Female
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum CivilState
        {
            [Display(Name = "Solteiro(a)")]
            [EnumMember(Value = "Solteiro(a)")]
            Single,

            [Display(Name = "Casado(a)")]
            [EnumMember(Value = "Casado(a)")]
            Married,

            [Display(Name = "Separado(a)")]
            [EnumMember(Value = "Separado(a)")]
            Separate,

            [Display(Name = "Divorciado(a)")]
            [EnumMember(Value = "Divorciado(a)")]
            Divorced,

            [Display(Name = "Viúvo(a)")]
            [EnumMember(Value = "Viúvo(a)")]
            Widowed,
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum PhoneType
        {
            [Display(Name = "Fixo")]
            [EnumMember(Value = "Fixo")]
            Landline,

            [Display(Name = "Celular")]
            [EnumMember(Value = "Celular")]
            Cellphone
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum ResidenceType
        {
            [Display(Name = "Própria")]
            [EnumMember(Value = "Própria")]
            Owner,

            [Display(Name = "Cedida")]
            [EnumMember(Value = "Cedida")]
            Ceded,

            [Display(Name = "Alugada")]
            [EnumMember(Value = "Alugada")]
            Leased,

            [Display(Name = "Outros")]
            [EnumMember(Value = "Outros")]
            Other
        }

        [JsonConverter(typeof(StringEnumConverter))]
        public enum ArchivePatientType
        {
            [Display(Name = "Alta")]
            [EnumMember(Value = "Alta")]
            Discharge,

            [Display(Name = "Óbito")]
            [EnumMember(Value = "Óbito")]
            Death
        }

        #endregion

        #region Sales Shirt 2020

        public enum Status
        {
            [Display(Name = "Pedido feito")]
            [EnumMember(Value = "Pedido feito")]
            Ordered = 5,

            [Display(Name = "Pagamento Recebido")]
            [EnumMember(Value = "Pagamento Recebido")]
            PaymentReceived = 10,

            [Display(Name = "Enviado para produção")]
            [EnumMember(Value = "Enviado para produção")]
            SentToProduction = 15,

            [Display(Name = "Produzido")]
            [EnumMember(Value = "Produzido")]
            Produced = 20,

            [Display(Name = "Retirado")]
            [EnumMember(Value = "Retirado")]
            Collected = 25,

            [Display(Name = "Cancelado")]
            [EnumMember(Value = "Cancelado")]
            Canceled = 30,
        }

        public enum ShirtType
        {
            [Display(Name = "Normal")]
            [EnumMember(Value = "Normal")]
            Normal = 5,

            [Display(Name = "Baby look")]
            [EnumMember(Value = "Baby look")]
            BabyLook = 10,
        }

        #endregion

        #region Helper

        public static string GetDisplayName(Enum enumValue)
        {
            return enumValue == null
                ? string.Empty
                : enumValue.GetType()
                    .GetMember(enumValue.ToString())
                    .First()
                    .GetCustomAttribute<DisplayAttribute>()
                    .GetName();
        }

        #endregion
    }
}
