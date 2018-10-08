using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static LigaCancer.Code.Globals;

namespace LigaCancer.Models.MedicalViewModels
{
    public class PhoneViewModel
    {
        public string Number { get; set; }

        public PhoneType PhoneType { get; set; }

        public string ObservationNote { get; set; }
    }
}
