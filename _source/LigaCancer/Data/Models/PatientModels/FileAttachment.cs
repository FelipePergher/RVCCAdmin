using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Data.Models.PatientModels
{
    public class FileAttachment : RegisterData
    {
        [Key]
        public int FileAttachmentId { get; set; }

        public string FilePath { get; set; }

    }
}