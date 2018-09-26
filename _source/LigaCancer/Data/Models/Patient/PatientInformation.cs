using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.Patient
{
    public class PatientInformation : RegisterData
    {
        public PatientInformation()
        {
            TreatmentPlaces = new HashSet<TreatmentPlace>();
            CancerTypes = new HashSet<CancerType>();
            Doctors = new HashSet<Doctor>();
            Medicines = new HashSet<Medicine>();
        }

        [Key]
        public int PatientInformationId { get; set; }

        public DateTime TreatmentbeginDate { get; set; }

        public ActivePatient ActivePatient { get; set; }


        public ICollection<Medicine> Medicines { get; set; }

        public ICollection<Doctor> Doctors { get; set; }

        public ICollection<CancerType> CancerTypes { get; set; }

        public ICollection<TreatmentPlace> TreatmentPlaces { get; set; }
    }
}