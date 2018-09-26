using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.Patient
{
    public class CancerType : RegisterData
    {
        [Key]
        public int CancerTypeId { get; set; }

        public string Name { get; set; }
    }
}