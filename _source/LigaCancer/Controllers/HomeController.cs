// <copyright file="HomeController.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.ViewModel;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminUserSocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly IDataRepository<Patient> _patientService;
        private readonly IDataRepository<Doctor> _doctorService;
        private readonly IDataRepository<TreatmentPlace> _treatmentPlaceService;
        private readonly IDataRepository<Medicine> _medicineService;
        private readonly IDataRepository<CancerType> _cancerTypeService;

        public HomeController(
            IDataRepository<Patient> patientService,
            IDataRepository<Doctor> doctorService,
            IDataRepository<TreatmentPlace> treatmentPlaceService,
            IDataRepository<Medicine> medicineService,
            IDataRepository<CancerType> cancerTypeService)
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
            var homeView = new HomeViewModel
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
