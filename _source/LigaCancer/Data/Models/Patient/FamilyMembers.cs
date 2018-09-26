using LigaCancer.Code;
using System.ComponentModel.DataAnnotations;
using static LigaCancer.Code.Globals;

namespace LigaCancer.Data.Models.Patient
{
    public class FamilyMembers : RegisterData
    {

        [Key]
        public int FamilyMemberId { get; set; }

        public string Name { get; set; }

        public string Kinship { get; set; }

        public int Age { get; set; }

        public Sex Sex { get; set; }

        public double MonthlyIncome { get; set; }
    }
}