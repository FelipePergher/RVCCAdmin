using LigaCancer.Data.Models.ManyToManyModels;
using System.Collections.Generic;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Profession : RegisterData
    {
        public int ProfessionId { get; set; }

        public string Name { get; set; }

        public ICollection<Patient> Patients { get; set; }
    }
}