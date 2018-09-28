using LigaCancer.Data.Models.ManyToManyModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class PatientInformation : RegisterData
    {
        [Key]
        public int PatientInformationId { get; set; }

        public DateTime TreatmentbeginDate { get; set; }

        public ActivePatient ActivePatient { get; set; }

        
        #region Relations

        public ICollection<PatientInformationCancerType> PatientInformationCancerTypes { get; set; }
        public ICollection<PatientInformationDoctor> PatientInformationDoctors { get; set; }
        public ICollection<PatientInformationTreatmentPlace> PatientInformationTreatmentPlaces { get; set; }
        public ICollection<PatientInformationMedicine> PatientInformationMedicines { get; set; }
        
        #endregion
    }
}