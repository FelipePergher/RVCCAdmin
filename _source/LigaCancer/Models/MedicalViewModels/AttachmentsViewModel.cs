using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.MedicalViewModels
{
    public class AttachmentsViewModel
    {
        public ICollection<string> PersonalDocuments { get; set; }
        public ICollection<string> MedicalDocuments { get; set; }
        public ICollection<string> OtherDocuments { get; set; }
    }
}
