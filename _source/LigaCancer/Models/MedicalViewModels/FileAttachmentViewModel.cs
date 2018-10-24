using LigaCancer.Code;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Models.MedicalViewModels
{
    public class FileAttachmentViewModel
    {
        public string PatientId { get; set; }

        public string FileAttachmentId { get; set; }

        [Display(Name = "Arquivo"), Required(ErrorMessage = "Este campo é obrigatório!")]
        public IFormFile File { get; set; }

        [Display(Name = "Nome do arquivo"), Required(ErrorMessage = "Este campo é obrigatório!")]
        public string FileName { get; set; }

        [Display(Name = "Categoria do arquivo")]
        public Globals.ArchiveCategorie FileCategory { get; set; }
    }
}
