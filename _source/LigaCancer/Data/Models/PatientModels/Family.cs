using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Family : RegisterData
    {
        public Family()
        {
            FamilyMembers = new HashSet<FamilyMembers>();
        }

        [Key]
        public int FamilyId { get; set; }

        public double MonthlyIncome { get; set; }

        public double FamilyIncome { get; set; }

        public double PerCapitaIncome { get; set; }

        public Residence Residence { get; set; }

        public ICollection<FamilyMembers> FamilyMembers { get; set; }
    }
}