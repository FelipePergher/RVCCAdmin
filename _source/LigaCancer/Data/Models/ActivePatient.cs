using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models
{
    public class ActivePatient : RegisterData
    {
        [Key]
        public int ActivePatientId { get; set; }

        public bool Death { get; set; }

        public bool Discharge { get; set; }

        public DateTime DeathDate { get; set; }

        public DateTime DischargeDate { get; set; }

        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
}