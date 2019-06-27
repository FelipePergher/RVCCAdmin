using Microsoft.AspNetCore.Mvc;

namespace LigaCancer.Models.SearchModel
{
    public class PhoneSearchModel
    {
        public PhoneSearchModel() { }

        public PhoneSearchModel(string patientId)
        {
            PatientId = patientId;
        }

        [HiddenInput]
        public string PatientId { get; set; }
    }
}
