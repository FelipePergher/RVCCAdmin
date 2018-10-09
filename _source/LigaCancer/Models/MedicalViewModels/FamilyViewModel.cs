using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.MedicalViewModels
{
    public class FamilyViewModel
    {
        public FamilyViewModel()
        {
            FamilyMembers = new List<FamilyMembersViewModel>();
        }

        public double MonthlyIncome { get; set; }

        public double FamilyIncome { get; set; }

        public double PerCapitaIncome { get; set; }

        public string Residence { get; set; }

        public List<FamilyMembersViewModel> FamilyMembers { get; set; }
    }
}
