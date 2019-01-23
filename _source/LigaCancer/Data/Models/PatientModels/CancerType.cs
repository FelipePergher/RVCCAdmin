using LigaCancer.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class CancerType : RegisterData
    {
        [Key]
        public int CancerTypeId { get; set; }

        [Display(Name = "Tipo do Câncer")]
        public string Name { get; set; }

        #region Relations

        public ICollection<PatientInformationCancerType> PatientInformationCancerTypes { get; set; }
        
        #endregion
    }
}