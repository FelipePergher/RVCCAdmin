using LigaCancer.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Doctor : RegisterData
    {
        [Key]
        public int DoctorId { get; set; }

        public string Name { get; set; }

        public string CRM { get; set; }

        #region Relations
        public ICollection<PatientInformationDoctor> PatientInformationDoctors { get; set; }
        #endregion
    }
}