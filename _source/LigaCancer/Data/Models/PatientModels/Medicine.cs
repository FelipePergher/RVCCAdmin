using LigaCancer.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Medicine : RegisterData
    {
        [Key]
        public int MedicineId { get; set; }

        [Display(Name = "Remédio")]
        public string Name { get; set; }

        #region Relations

        public ICollection<PatientInformationMedicine> PatientInformationMedicines { get; set; }

        #endregion
    }
}