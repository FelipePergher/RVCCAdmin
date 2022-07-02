// <copyright file="IFileAttachment.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

namespace RVCC.Data.Interfaces
{
    public interface IFileAttachment
    {
        public int FileAttachmentId { get; set; }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public double FileSize { get; set; }

        public string FilePath { get; set; }

        public int PatientId { get; set; }
    }
}