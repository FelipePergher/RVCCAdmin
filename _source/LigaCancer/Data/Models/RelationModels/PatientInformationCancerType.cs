using RVCC.Data.Models.PatientModels;

namespace RVCC.Data.Models.RelationModels
{
    public class PatientInformationCancerType
    {
        public PatientInformationCancerType() { }

        public PatientInformationCancerType(CancerType cancerType)
        {
            CancerType = cancerType;
        }

        public int PatientInformationId { get; set; }
        public PatientInformation PatientInformation { get; set; }

        public int CancerTypeId { get; set; }
        public CancerType CancerType { get; set; }
    }
}
