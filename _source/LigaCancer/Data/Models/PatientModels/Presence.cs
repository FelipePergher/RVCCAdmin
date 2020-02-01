using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.PatientModels
{
    public class Presence : RegisterData
    {
        [Key]
        public int PresenceId { get; set; }

        public int PatientId { get; set; }

        public string Name { get; set; }

        public DateTime PresenceDateTime { get; set; }
    }
}
