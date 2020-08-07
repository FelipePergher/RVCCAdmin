// <copyright file="FileAttachment.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RVCC.Data.Models
{
    public class FileAttachment : RegisterData
    {
        [Key]
        public int FileAttachmentId { get; set; }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public double FileSize { get; set; }

        public string FilePath { get; set; }

        public int PatientId { get; set; }

        [ForeignKey(nameof(PatientId))]
        public virtual Patient Patient { get; set; }
    }
}