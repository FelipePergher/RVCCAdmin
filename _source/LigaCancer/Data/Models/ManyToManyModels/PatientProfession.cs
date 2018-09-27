using LigaCancer.Data.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Models.ManyToManyModels
{
    public class PatientProfession
    {
        public int PatientId { get; set; }
        public Patient Patient { get; set; }

        public int ProfessionId { get; set; }
        public Profession  Profession { get; set; }
    }
}
