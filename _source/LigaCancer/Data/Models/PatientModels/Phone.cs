using LigaCancer.Code;
using System.ComponentModel.DataAnnotations;
using static LigaCancer.Code.Globals;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Phone : RegisterData
    {
        [Key]
        public int PhoneId { get; set; }

        public string Number { get; set; }

        public PhoneType PhoneType { get; set; }

        public string ObservationNote { get; set; }
    }
}