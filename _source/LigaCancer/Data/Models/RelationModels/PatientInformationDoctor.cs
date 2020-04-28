namespace RVCC.Data.Models.RelationModels
{
    public class PatientInformationDoctor
    {
        public PatientInformationDoctor() { }

        public PatientInformationDoctor(Doctor doctor)
        {
            Doctor = doctor;
        }

        public int PatientInformationId { get; set; }
        public PatientInformation PatientInformation { get; set; }

        public int DoctorId { get; set; }
        public Doctor Doctor { get; set; }
    }
}
