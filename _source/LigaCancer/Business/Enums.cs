// <copyright file="Enums.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

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

        public enum Sex
        {
            [Display(Name = "Não especificado")]
            [EnumMember(Value = "Não especificado")]
            NotSpecified = 0,

            [Display(Name = "Homem")]
            [EnumMember(Value = "Homem")]
            Male = 1,

            [Display(Name = "Mulher")]
            [EnumMember(Value = "Mulher")]
            Female = 2
        }

        public enum CivilState
        {
            [Display(Name = "Solteiro(a)")]
            [EnumMember(Value = "Solteiro(a)")]
            Single = 0,

            [Display(Name = "Casado(a)")]
            [EnumMember(Value = "Casado(a)")]
            Married = 1,

            [Display(Name = "Separado(a)")]
            [EnumMember(Value = "Separado(a)")]
            Separate = 2,

            [Display(Name = "Divorciado(a)")]
            [EnumMember(Value = "Divorciado(a)")]
            Divorced = 3,

            [Display(Name = "Viúvo(a)")]
            [EnumMember(Value = "Viúvo(a)")]
            Widowed = 4
        }

        public enum PhoneType
        {
            [Display(Name = "Fixo")]
            [EnumMember(Value = "Fixo")]
            Landline = 0,

            [Display(Name = "Celular")]
            [EnumMember(Value = "Celular")]
            Cellphone = 1
        }

        public enum ResidenceType
        {
            [Display(Name = "Própria")]
            [EnumMember(Value = "Própria")]
            Owner = 0,

            [Display(Name = "Cedida")]
            [EnumMember(Value = "Cedida")]
            Ceded = 1,

            [Display(Name = "Alugada")]
            [EnumMember(Value = "Alugada")]
            Leased = 2,

            [Display(Name = "Outros")]
            [EnumMember(Value = "Outros")]
            Other = 3
        }

        public enum ArchivePatientType
        {
            [Display(Name = "Alta")]
            [EnumMember(Value = "Alta")]
            Discharge = 0,

            [Display(Name = "Mudou-se")]
            [EnumMember(Value = "Mudou-se")]
            ResidenceChange = 1,

            [Display(Name = "Óbito")]
            [EnumMember(Value = "Óbito")]
            Death = 2
        }

        public enum ExpenseTypeFrequency
        {
            [Display(Name = "Mensal")]
            [EnumMember(Value = "Mensal")]
            Montlhy = 0,

            [Display(Name = "Anual")]
            [EnumMember(Value = "Anual")]
            Yearly = 1,

            [Display(Name = "Total")]
            [EnumMember(Value = "Total")]
            Total = 2
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
