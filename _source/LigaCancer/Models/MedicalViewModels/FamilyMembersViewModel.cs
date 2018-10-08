using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LigaCancer.Code.Globals;

namespace LigaCancer.Models.MedicalViewModels
{
    public class FamilyMembersViewModel
    {
        public string Name { get; set; }

        public string Kinship { get; set; }

        public int Age { get; set; }

        public Sex Sex { get; set; }

        public double MonthlyIncome { get; set; }
    }
}
