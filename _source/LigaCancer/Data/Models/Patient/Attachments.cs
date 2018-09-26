using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.Patient
{
    public class Attachments : RegisterData
    {
        public Attachments()
        {
            PersonalDocuments = new HashSet<FileAttachment>();
            MedicalDocuments = new HashSet<FileAttachment>();
            OtherDocuments = new HashSet<FileAttachment>();
        }

        [Key]
        public int AttachmentsId { get; set; }

        public ICollection<FileAttachment> PersonalDocuments { get; set; }
        public ICollection<FileAttachment> MedicalDocuments { get; set; }
        public ICollection<FileAttachment> OtherDocuments { get; set; }
    }
}