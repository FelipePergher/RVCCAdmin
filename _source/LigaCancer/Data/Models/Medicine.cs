using RVCC.Data.Models.RelationModels;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models
{
    public class Medicine : RegisterData
    {
        public Medicine() { }

        public Medicine(string name, ApplicationUser user)
        {
            Name = name;
            CreatedBy = user.Name;
        }

        [Key]
        public int MedicineId { get; set; }

        [Display(Name = "Remédio")]
        public string Name { get; set; }

        #region Relations

        public ICollection<PatientInformationMedicine> PatientInformationMedicines { get; set; }

        #endregion
    }
}