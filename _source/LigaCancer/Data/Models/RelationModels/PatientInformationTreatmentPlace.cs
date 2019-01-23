using LigaCancer.Data.Models.PatientModels;

namespace LigaCancer.Data.Models.RelationModels
{
    public class PatientInformationTreatmentPlace
    {
        public int PatientInformationId { get; set; }
        public PatientInformation PatientInformation { get; set; }

        public int TreatmentPlaceId { get; set; }
        public TreatmentPlace TreatmentPlace { get; set; }
    }
}
