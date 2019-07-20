using LigaCancer.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Doctor : RegisterData
    {
        public Doctor() { }

        public Doctor(string name, ApplicationUser user)
        {
            Name = name;
            CreatedBy = user.Name;
        }

        public Doctor(string name, string crm, ApplicationUser user)
        {
            Name = name;
            CRM = crm;
            CreatedBy = user.Name;
        }

        [Key]
        public int DoctorId { get; set; }

        public string Name { get; set; }

        public string CRM { get; set; }

        #region Relations
        public ICollection<PatientInformationDoctor> PatientInformationDoctors { get; set; }
        #endregion
    }
}