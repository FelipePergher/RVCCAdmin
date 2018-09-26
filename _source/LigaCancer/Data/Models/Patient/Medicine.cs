using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.Patient
{
    public class Medicine : RegisterData
    {
        [Key]
        public int MedicineId { get; set; }

        public string Name { get; set; }
    }
}