using LigaCancer.Data.Models.PatientModels;

namespace LigaCancer.Data.Models.RelationModels
{
    public class PatientInformationMedicine
    {
        public int PatientInformationId { get; set; }
        public PatientInformation PatientInformation { get; set; }

        public int MedicineId { get; set; }
        public Medicine Medicine { get; set; }
    }
}
