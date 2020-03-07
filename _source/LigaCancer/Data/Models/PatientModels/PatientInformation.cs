using RVCC.Data.Models.RelationModels;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models.PatientModels
{
    public class PatientInformation : RegisterData
    {
        public PatientInformation()
        {
            PatientInformationCancerTypes = new List<PatientInformationCancerType>();
            PatientInformationDoctors = new List<PatientInformationDoctor>();
            PatientInformationMedicines = new List<PatientInformationMedicine>();
            PatientInformationTreatmentPlaces = new List<PatientInformationTreatmentPlace>();
        }

        [Key]
        public int PatientInformationId { get; set; }

        public DateTime TreatmentBeginDate { get; set; }

        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }

        #region Relations

        public List<PatientInformationCancerType> PatientInformationCancerTypes { get; set; }

        public List<PatientInformationDoctor> PatientInformationDoctors { get; set; }

        public List<PatientInformationTreatmentPlace> PatientInformationTreatmentPlaces { get; set; }

        public List<PatientInformationMedicine> PatientInformationMedicines { get; set; }

        #endregion
    }
}