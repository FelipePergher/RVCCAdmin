using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.Patient
{
    public class TreatmentPlace : RegisterData
    {
        [Key]
        public int TreatmentPlaceId { get; set; }

        public string City { get; set; }

    }
}