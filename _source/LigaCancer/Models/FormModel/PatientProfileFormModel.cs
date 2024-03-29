﻿// <copyright file="PatientProfileFormModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using RVCC.Business;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class PatientProfileFormModel
    {
        public PatientProfileFormModel()
        {
        }

        public PatientProfileFormModel(int patientId)
        {
            PatientId = patientId;
        }

        [HiddenInput]
        public int PatientId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [StringLength(150, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Surname { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsRgExist", "PatientApi", AdditionalFields = "PatientId", ErrorMessage = "RG já registrado!")]
        [StringLength(20, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string RG { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsCpfExist", "PatientApi", AdditionalFields = "PatientId", ErrorMessage = "CPF já registrado!")]
        [StringLength(14, MinimumLength = 14, ErrorMessage = "CPF inválido")]
        public string CPF { get; set; }

        [Display(Name = "Necessidades Imediatas")]
        public string ImmediateNecessities { get; set; }

        public DateTime ImmediateNecessitiesDateUpdated { get; set; }

        [Display(Name = "Grupo de Convivência")]
        public bool FamiliarityGroup { get; set; }

        [Display(Name = "Encaminhado para casa de apoio")]
        public bool ForwardedToSupportHouse { get; set; }

        [Display(Name = "Gênero")]
        public Enums.Sex Sex { get; set; }

        [Display(Name = "Estado Civil")]
        public Enums.CivilState? CivilState { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DateRange]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string DateOfBirth { get; set; }

        [Display(Name = "Data de Ingresso")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DateRange]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string JoinDate { get; set; }

        [Display(Name = "Profissão")]
        [StringLength(100, ErrorMessage = "O {0} deve ter no máximo {1} caracteres.")]
        public string Profession { get; set; }

        [Display(Name = "Renda mensal")]
        public string MonthlyIncome { get; set; }
    }
}
