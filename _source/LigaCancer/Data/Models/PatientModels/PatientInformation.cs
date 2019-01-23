using LigaCancer.Data.Models.RelationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class PatientInformation : RegisterData
    {
        public PatientInformation()
        {
            PatientInformationCancerTypes = new List<PatientInformationCancerType>();
            PatientInformationDoctors = new List<PatientInformationDoctor>();
            PatientInformationMedicines = new List<PatientInformationMedicine>();
            PatientInformationTreatmentPlaces = new List<PatientInformationTreatmentPlace>();
            ActivePatient = new ActivePatient();
        }

        [Key]
        public int PatientInformationId { get; set; }

        public DateTime TreatmentbeginDate { get; set; }

        public ActivePatient ActivePatient { get; set; }
        
        #region Relations

        public List<PatientInformationCancerType> PatientInformationCancerTypes { get; set; }
        
        public List<PatientInformationDoctor> PatientInformationDoctors { get; set; }
        
        public List<PatientInformationTreatmentPlace> PatientInformationTreatmentPlaces { get; set; }
        
        public List<PatientInformationMedicine> PatientInformationMedicines { get; set; }
        
        #endregion
    }
}