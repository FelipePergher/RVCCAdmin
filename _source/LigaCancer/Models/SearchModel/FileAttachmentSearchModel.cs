using Microsoft.AspNetCore.Mvc;

namespace LigaCancer.Models.SearchModel
{
    public class FileAttachmentSearchModel
    {
        public FileAttachmentSearchModel() { }

        public FileAttachmentSearchModel(string patientId)
        {
            PatientId = patientId;
        }

        [HiddenInput]
        public string PatientId { get; set; }
    }
}
