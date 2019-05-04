using LigaCancer.Code;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LigaCancer.Data.Models.PatientModels
{
    public class FamilyMember : RegisterData
    {
        [Key]
        public int FamilyMemberId { get; set; }

        public string Name { get; set; }

        public string Kinship { get; set; }

        public int? Age { get; set; }

        public Globals.Sex Sex { get; set; }

        public double MonthlyIncome { get; set; }

        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
}