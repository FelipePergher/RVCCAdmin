using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.Patient
{
    public class Residence : RegisterData
    {
        [Key]
        public int ResidenceId { get; set; }

        public ResidenceType ResidenceType { get; set; }

        public string ResidenceObservation { get; set; }
    }
}