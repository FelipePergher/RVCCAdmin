using LigaCancer.Data.Models.ManyToManyModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Residence : RegisterData
    {
        [Key]
        public int ResidenceId { get; set; }

        public string ResidenceObservation { get; set; }

        #region Relations

        public ICollection<ResidenceTypeResidence> ResidenceTypeResidences { get; set; }

        #endregion
    }
}