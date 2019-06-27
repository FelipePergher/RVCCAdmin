using Microsoft.AspNetCore.Mvc;

namespace LigaCancer.Models.SearchModel
{
    public class FamilyMemberSearchModel
    {
        public FamilyMemberSearchModel() { }

        public FamilyMemberSearchModel(string patientId)
        {
            PatientId = patientId;
        }

        [HiddenInput]
        public string PatientId { get; set; }
    }
}
