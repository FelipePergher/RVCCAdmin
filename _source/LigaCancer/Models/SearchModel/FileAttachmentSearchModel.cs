// <copyright file="FileAttachmentSearchModel.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;

namespace RVCC.Models.SearchModel
{
    public class FileAttachmentSearchModel
    {
        public FileAttachmentSearchModel()
        {
        }

        public FileAttachmentSearchModel(string patientId)
        {
            PatientId = patientId;
        }

        [HiddenInput]
        public string PatientId { get; set; }
    }
}
