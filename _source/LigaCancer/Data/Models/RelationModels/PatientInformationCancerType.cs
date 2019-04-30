using LigaCancer.Data.Models.PatientModels;

namespace LigaCancer.Data.Models.RelationModels
{
    public class PatientInformationCancerType
    {
        public PatientInformationCancerType(){}

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
