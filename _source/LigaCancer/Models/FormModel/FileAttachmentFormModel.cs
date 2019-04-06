using LigaCancer.Code;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace LigaCancer.Models.FormModel
{
    public class FileAttachmentFormModel
    {
        public string PatientId { get; set; }

        public string FileAttachmentId { get; set; }

        [Display(Name = "Arquivo")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public IFormFile File { get; set; }

        [Display(Name = "Nome do arquivo")]
        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string FileName { get; set; }

        [Display(Name = "Categoria do arquivo")]
        public Globals.ArchiveCategorie FileCategory { get; set; }
    }
}
