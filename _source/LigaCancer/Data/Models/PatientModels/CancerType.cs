using LigaCancer.Data.Models.ManyToManyModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class CancerType : RegisterData
    {
        [Key]
        public int CancerTypeId { get; set; }

        public string Name { get; set; }

        #region Relations

        public ICollection<PatientInformationCancerType> PatientInformationCancerTypes { get; set; }
        
        #endregion
    }
}