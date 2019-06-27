using Microsoft.AspNetCore.Mvc;

namespace LigaCancer.Models.SearchModel
{
    public class AddressSearchModel
    {
        public AddressSearchModel() { }

        public AddressSearchModel(string patientId)
        {
            PatientId = patientId;
        }

        [HiddenInput]
        public string PatientId { get; set; }
    }
}
