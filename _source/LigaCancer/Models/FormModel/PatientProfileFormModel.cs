﻿using LigaCancer.Code;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class PatientProfileFormModel
    {
        public PatientProfileFormModel(){ }

        public PatientProfileFormModel(int patientId)
        {
            PatientId = patientId;
        }

        [HiddenInput]
        public int PatientId { get; set; }

        [Display(Name = "Nome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string FirstName { get; set; }

        [Display(Name = "Sobrenome")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string Surname { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsRgExist", "PatientApi", AdditionalFields = "PatientId", ErrorMessage = "RG já registrado!")]
        public string RG { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [Remote("IsCpfExist", "PatientApi", AdditionalFields = "PatientId", ErrorMessage = "CPF já registrado!")]
        public string CPF { get; set; }

        [Display(Name = "Grupo de Convivência")]
        public bool FamiliarityGroup { get; set; }

        [Display(Name = "Gênero")]
        public Globals.Sex Sex { get; set; }

        [Display(Name = "Estado Civil")]
        public Globals.CivilState? CivilState { get; set; }

        [Display(Name = "Data de Nascimento")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DateRange]
        public DateTime? DateOfBirth { get; set; }

        [Display(Name = "Profissão")]
        public string Profession { get; set; }

        [Display(Name = "Renda mensal")]
        [Range(0, 1000000.00, ErrorMessage = "Insira um valor de 0.00 até 1,000,000")]
        [RegularExpression(@"[0-9]{0,5}\,?[0-9]{1,2}", ErrorMessage = "Insira um valor válido!")]
        public double? MonthlyIncome { get; set; }

    }
}
