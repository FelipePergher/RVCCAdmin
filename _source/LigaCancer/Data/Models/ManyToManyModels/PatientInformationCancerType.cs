using LigaCancer.Data.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Models.ManyToManyModels
{
    public class PatientInformationCancerType
    {
        public int PatientInformationId { get; set; }
        public PatientInformation PatientInformation { get; set; }

        public int CancerTypeId { get; set; }
        public CancerType CancerType { get; set; }
    }
}
