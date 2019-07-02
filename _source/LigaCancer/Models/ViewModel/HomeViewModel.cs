using System;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.ViewModel
{
    public class HomeViewModel
    {
        public HomeViewModel()
        {
            ChartDate = DateTime.Now;
        }

        public int PatientCount { get; set; }

        public int DoctorCount { get; set; }

        public int CancerTypeCount { get; set; }

        public int MedicineCount { get; set; }

        public int TreatmentPlaceCount { get; set; }

        [Display(Name = "Data dos gráfico")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime ChartDate { get; set; }
    }
}
