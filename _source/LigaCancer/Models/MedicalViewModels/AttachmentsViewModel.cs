using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.MedicalViewModels
{
    public class AttachmentsViewModel
    {
        public AttachmentsViewModel()
        {
            PersonalDocuments = new List<string>();
            MedicalDocuments = new List<string>();
            OtherDocuments = new List<string>();
        }

        public List<string> PersonalDocuments { get; set; }
        public List<string> MedicalDocuments { get; set; }
        public List<string> OtherDocuments { get; set; }
    }
}
