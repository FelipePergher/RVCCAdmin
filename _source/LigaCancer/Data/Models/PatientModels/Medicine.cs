using LigaCancer.Data.Models.ManyToManyModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Medicine : RegisterData
    {
        [Key]
        public int MedicineId { get; set; }

        public string Name { get; set; }

        #region Relations

        public ICollection<PatientInformationMedicine> PatientInformationMedicines { get; set; }

        #endregion
    }
}