// <copyright file="AuditFileAttachment.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using RVCC.Data.Interfaces;
using System;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Data.Models.Audit
{
    public class AuditFileAttachment : IAudit, IFileAttachment
    {
        [Key]
        public int AuditFileAttachmentId { get; set; }

        #region IAudit

        public int FileAttachmentId { get; set; }

        public string FileName { get; set; }

        public string FileExtension { get; set; }

        public double FileSize { get; set; }

        public string FilePath { get; set; }

        public int PatientId { get; set; }

        #endregion

        #region IAudit

        public DateTime AuditDate { get; set; }

        public string UserName { get; set; }

        public string AuditAction { get; set; }

        #endregion
    }
}