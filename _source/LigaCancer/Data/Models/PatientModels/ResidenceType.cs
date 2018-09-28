using LigaCancer.Data.Models.ManyToManyModels;
using System.Collections.Generic;

namespace LigaCancer.Data.Models.PatientModels
{
    public class ResidenceType : RegisterData
    {
        public int ResidenceTypeId { get; set; }

        public string Type { get; set; }


        #region Relations

        public ICollection<ResidenceTypeResidence> ResidenceTypeResidences { get; set; }

        #endregion

    }
}