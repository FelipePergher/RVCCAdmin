using System.ComponentModel.DataAnnotations;
using LigaCancer.Code;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Phone : RegisterData
    {
        [Key]
        public int PhoneId { get; set; }

        public string Number { get; set; }

        public Globals.PhoneType PhoneType { get; set; }

        public string ObservationNote { get; set; }
    }
}