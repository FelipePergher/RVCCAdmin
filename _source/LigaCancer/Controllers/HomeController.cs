using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Code.Interface;
using LigaCancer.Models.ViewModel;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin"), AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly IDataStore<Patient> _patientService;
        private readonly IDataStore<Doctor> _doctorService;
        private readonly IDataStore<TreatmentPlace> _treatmentPlaceService;
        private readonly IDataStore<Medicine> _medicineService;
        private readonly IDataStore<CancerType> _cancerTypeService;

        public HomeController(
            IDataStore<Patient> patientService,
            IDataStore<Doctor> doctorService,
            IDataStore<TreatmentPlace> treatmentPlaceService,
            IDataStore<Medicine> medicineService,
            IDataStore<CancerType> cancerTypeService
            )
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _treatmentPlaceService = treatmentPlaceService;
            _medicineService = medicineService;
            _cancerTypeService = cancerTypeService;
        }

        public IActionResult Index()
        {
            HomeViewModel homeView = new HomeViewModel
            {
              CancerTypeCount = _cancerTypeService.Count(),
              DoctorCount = _doctorService.Count(),
              MedicineCount = _medicineService.Count(),
              PatientCount = _patientService.Count(),
              TreatmentPlaceCount = _treatmentPlaceService.Count()
            };

            return View(homeView);
        }

    }
}
