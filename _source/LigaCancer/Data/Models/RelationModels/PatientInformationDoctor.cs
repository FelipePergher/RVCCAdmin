using LigaCancer.Data.Models.PatientModels;

namespace LigaCancer.Data.Models.RelationModels
{
    public class PatientInformationDoctor
    {
        public int PatientInformationId { get; set; }
        public PatientInformation PatientInformation { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
