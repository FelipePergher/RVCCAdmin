using System.ComponentModel.DataAnnotations;
using System;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Presence : RegisterData
    {
        [Key]
        public int PresenceId { get; set; }

        public Patient Patient { get; set; }

        public DateTime PresenceDateTime { get; set; }
    }
}
