// <copyright file="HomeViewModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.ViewModel
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            ChartDate = DateTime.Now.ToString("dd/MM/yyyy");
        }

        public int PatientCount { get; set; }

        public int DoctorCount { get; set; }

        public int CancerTypeCount { get; set; }

        public int MedicineCount { get; set; }

        public int TreatmentPlaceCount { get; set; }

        [Display(Name = "Data do gráfico")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [DataType(DataType.Date)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        public string ChartDate { get; set; }
    }
}
