using LigaCancer.Code;
using System.ComponentModel.DataAnnotations;

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