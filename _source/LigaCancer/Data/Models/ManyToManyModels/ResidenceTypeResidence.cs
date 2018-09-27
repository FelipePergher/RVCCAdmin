using LigaCancer.Data.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Models.ManyToManyModels
{
    public class ResidenceTypeResidence
    {
        public int ResidenceTypeId { get; set; }
        public ResidenceType ResidenceType { get; set; }

        public int ResidenceId { get; set; }
        public Residence Residence { get; set; }
    }
}
