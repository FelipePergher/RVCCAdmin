using LigaCancer.Code;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LigaCancer.Data.Models.PatientModels
{
    public class FileAttachment : RegisterData
    {
        [Key]
        public int FileAttachmentId { get; set; }

        public string FileName { get; set; }

        public string FilePath { get; set; }

        public Globals.ArchiveCategorie ArchiveCategorie { get; set; }

        public int PatientId { get; set; }

        [ForeignKey("PatientId")]
        public virtual Patient Patient { get; set; }
    }
}