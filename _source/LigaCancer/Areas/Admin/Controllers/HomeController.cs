using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaCancer.Areas.Admin.Controllers 
{
    [Authorize(Roles = "Admin, User")]
    [AutoValidateAntiforgeryToken]
    [Area("Admin")]
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

        [HttpGet]
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
