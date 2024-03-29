﻿// <copyright file="PatientSearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc.Rendering;
using RVCC.Business;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.SearchModel
{
    public class PatientSearchModel
    {
        public PatientSearchModel()
        {
            CancerTypes = new List<string>();
            Doctors = new List<string>();
            TreatmentPlaces = new List<string>();
            Medicines = new List<string>();
            ServiceTypes = new List<string>();
            PatientAuxiliarAccessoryTypes = new List<string>();
            PatientTreatmentTypes = new List<string>();
        }

        [Display(Name = "Nome")]
        public string Name { get; set; }

        [Display(Name = "Sobrenome")]
        public string Surname { get; set; }

        [Display(Name = "RG")]
        public string Rg { get; set; }

        [Display(Name = "CPF")]
        public string Cpf { get; set; }

        [Display(Name = "Estado civil")]
        public string CivilState { get; set; }

        [Display(Name = "Gênero")]
        public string Sex { get; set; }

        [Display(Name = "Grupo de convivência")]
        public string FamiliarityGroup { get; set; }

        [Display(Name = "Encaminhado Casa de apoio")]
        public string ForwardedToSupportHouse { get; set; }

        [Display(Name = "Status")]
        public Enums.ArchivePatientType? ArchivePatientType { get; set; }

        [Display(Name = "Cânceres")]
        public List<string> CancerTypes { get; set; }

        [Display(Name = "Remédios")]
        public List<string> Medicines { get; set; }

        [Display(Name = "Médicos")]
        public List<string> Doctors { get; set; }

        [Display(Name = "Locais de Tratamentos")]
        public List<string> TreatmentPlaces { get; set; }

        [Display(Name = "Serviços")]
        public List<string> ServiceTypes { get; set; }

        [Display(Name = "Acessórios Auxiliares")]
        public List<string> PatientAuxiliarAccessoryTypes { get; set; }

        [Display(Name = "Tratamentos")]
        public List<string> PatientTreatmentTypes { get; set; }

        public List<SelectListItem> FamiliarityGroups => new List<SelectListItem>
        {
            new SelectListItem(string.Empty, string.Empty),
            new SelectListItem("Participa", "true"),
            new SelectListItem("Não Participa", "false")
        };

        public List<SelectListItem> ForwardedToSupportHouseOptions => new List<SelectListItem>
        {
            new SelectListItem(string.Empty, string.Empty),
            new SelectListItem("Encaminhado", "true"),
            new SelectListItem("Não encaminhado", "false")
        };

        [Display(Name = "Data Nascimento Inicial")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        [DataType(DataType.Date)]
        public string BirthdayDateFrom { get; set; }

        [Display(Name = "Data Nascimento Final")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string BirthdayDateTo { get; set; }

        [Display(Name = "Data Ingresso Inicial")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        [DataType(DataType.Date)]
        public string JoinDateFrom { get; set; }

        [Display(Name = "Data Ingresso Final")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string JoinDateTo { get; set; }
    }
}
