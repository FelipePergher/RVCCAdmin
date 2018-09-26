using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.Patient
{
    public class Doctor : RegisterData
    {
        [Key]
        public int DoctorId { get; set; }

        public string Name { get; set; }

        public string CRM { get; set; } 
    }
}