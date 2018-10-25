using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Models;
using Microsoft.AspNetCore.Authorization;
using LigaCancer.Data.Store;
using LigaCancer.Data.Models.PatientModels;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin")]
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
            HomeViewModel homeViewModel = new HomeViewModel
            {
              CancerTypeCount = _cancerTypeService.Count(),
              DoctorCount = _doctorService.Count(),
              MedicineCount = _medicineService.Count(),
              PatientCount = _patientService.Count(),
              TreatmentPlaceCount = _treatmentPlaceService.Count()
            };

            return View(homeViewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
