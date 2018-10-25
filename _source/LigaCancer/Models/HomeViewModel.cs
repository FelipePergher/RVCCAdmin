using LigaCancer.Data.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models
{
    public class HomeViewModel
    {
        public int PatientCount { get; set; }
        public int DoctorCount { get; set; }
        public int CancerTypeCount { get; set; }
        public int MedicineCount { get; set; }
        public int TreatmentPlaceCount { get; set; }
    }
}
