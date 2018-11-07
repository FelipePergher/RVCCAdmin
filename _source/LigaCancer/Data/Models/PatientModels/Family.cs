using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class Family : RegisterData
    {
        public Family()
        {
            FamilyMembers = new HashSet<FamilyMember>();
        }

        [Key]
        public int FamilyId { get; set; }

        public double FamilyIncome { get; set; }

        public double PerCapitaIncome { get; set; }

        public double MonthlyIncome { get; set; }

        public ICollection<FamilyMember> FamilyMembers { get; set; }
    }
}