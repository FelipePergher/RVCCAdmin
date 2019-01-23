using System.Collections.Generic;

namespace LigaCancer.Models.FormViewModel
{
    public class FamilyViewModel
    {
        public FamilyViewModel()
        {
            FamilyMembers = new List<FamilyMemberViewModel>();
        }

        public double MonthlyIncome { get; set; }

        public double FamilyIncome { get; set; }

        public double PerCapitaIncome { get; set; }

        public string Residence { get; set; }

        public List<FamilyMemberViewModel> FamilyMembers { get; set; }
    }
}
