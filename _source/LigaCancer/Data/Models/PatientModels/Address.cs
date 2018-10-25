using LigaCancer.Code;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Address : RegisterData
    {
        [Key]
        public int AddressId { get; set; }

        public string Street { get; set; }

        public string Neighborhood { get; set; }

        public string City { get; set; }

        public string HouseNumber { get; set; }

        public string Complement { get; set; }

        public string ObservationAddress { get; set; }

        public Globals.ResidenceType? ResidenceType { get; set; }

        public double MonthlyAmmountResidence { get; set; }
    }
}