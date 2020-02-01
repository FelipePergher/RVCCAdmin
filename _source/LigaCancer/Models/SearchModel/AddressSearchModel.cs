using Microsoft.AspNetCore.Mvc;

namespace RVCC.Models.SearchModel
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
