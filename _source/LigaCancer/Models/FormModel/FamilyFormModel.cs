using System.Collections.Generic;

namespace LigaCancer.Models.FormModel
{
    public class FamilyFormModel
    {
        public FamilyFormModel()
        {
            FamilyMembers = new List<FamilyMemberFormModel>();
        }

        public double MonthlyIncome { get; set; }

        public double FamilyIncome { get; set; }

        public double PerCapitaIncome { get; set; }

        public string Residence { get; set; }

        public List<FamilyMemberFormModel> FamilyMembers { get; set; }
    }
}
