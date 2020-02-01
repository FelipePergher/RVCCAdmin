using Microsoft.AspNetCore.Mvc.Rendering;
using RVCC.Business;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace RVCC.Models.FormModel
{
    public class PatientInformationFormModel
    {
        public PatientInformationFormModel()
        {
            CancerTypes = new List<string>();
            Medicines = new List<string>();
            Doctors = new List<string>();
            TreatmentPlaces = new List<string>();
        }

        [Display(Name = "Data de início do tratameto")]
        [DisplayFormat(DataFormatString = @"{0:dd\/MM\/yyyy}", ApplyFormatInEditMode = true)]
        [RegularExpression(@"^([0-2][0-9]|(3)[0-1])(\/)(((0)[0-9])|((1)[0-2]))(\/)\d{4}$", ErrorMessage = "Insira uma data válida")]
        [DataType(DataType.Date)]
        [DateRange]
        public string TreatmentBeginDate { get; set; }

        [Display(Name = "Cânceres")]
        public List<string> CancerTypes { get; set; }

        [Display(Name = "Médicos")]
        public List<string> Doctors { get; set; }

        [Display(Name = "Locais de tratamento")]
        public List<string> TreatmentPlaces { get; set; }

        [Display(Name = "Remédios")]
        public List<string> Medicines { get; set; }

        #region Selects

        public List<SelectListItem> SelectDoctors { get; set; }

        public List<SelectListItem> SelectTreatmentPlaces { get; set; }

        public List<SelectListItem> SelectMedicines { get; set; }

        public List<SelectListItem> SelectCancerTypes { get; set; }

        #endregion
    }
}
