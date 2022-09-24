// <copyright file="HomeController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Data.Models.RelationModels;
using RVCC.Models.ViewModel;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class HomeController : Controller
    {
        private readonly IDataRepository<Patient> _patientService;
        private readonly IDataRepository<Doctor> _doctorService;
        private readonly IDataRepository<TreatmentPlace> _treatmentPlaceService;
        private readonly IDataRepository<Medicine> _medicineService;
        private readonly IDataRepository<CancerType> _cancerTypeService;
        private readonly IDataRepository<Visitor> _visitorService;
        private readonly IDataRepository<Stay> _stayService;
        private readonly IDataRepository<PatientBenefit> _patientBenefitService;
        private readonly IDataRepository<Benefit> _benefitService;
        private readonly IDataRepository<ExpenseType> _expenseTypeService;
        private readonly IDataRepository<ServiceType> _serviceTypeService;
        private readonly IDataRepository<Attendant> _attendantService;
        private readonly IDataRepository<AttendanceType> _attendanceTypeService;
        private readonly IDataRepository<VisitorAttendanceType> _visitorAttendanceTypeTypeService;
        private readonly IDataRepository<AuxiliarAccessoryType> _auxiliarAccessoryTypeService;

        public HomeController(
            IDataRepository<Patient> patientService,
            IDataRepository<Doctor> doctorService,
            IDataRepository<TreatmentPlace> treatmentPlaceService,
            IDataRepository<Medicine> medicineService,
            IDataRepository<CancerType> cancerTypeService,
            IDataRepository<Visitor> visitorService,
            IDataRepository<Stay> stayService,
            IDataRepository<PatientBenefit> patientBenefitService,
            IDataRepository<Benefit> benefitService,
            IDataRepository<ExpenseType> expenseTypeService,
            IDataRepository<ServiceType> serviceTypeService,
            IDataRepository<Attendant> attendantService,
            IDataRepository<AttendanceType> attendanceTypeService,
            IDataRepository<VisitorAttendanceType> visitorAttendanceTypeTypeService,
            IDataRepository<AuxiliarAccessoryType> auxiliarAccessoryTypeService)
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _treatmentPlaceService = treatmentPlaceService;
            _medicineService = medicineService;
            _cancerTypeService = cancerTypeService;
            _visitorService = visitorService;
            _stayService = stayService;
            _patientBenefitService = patientBenefitService;
            _benefitService = benefitService;
            _expenseTypeService = expenseTypeService;
            _serviceTypeService = serviceTypeService;
            _attendantService = attendantService;
            _attendanceTypeService = attendanceTypeService;
            _visitorAttendanceTypeTypeService = visitorAttendanceTypeTypeService;
            _auxiliarAccessoryTypeService = auxiliarAccessoryTypeService;
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
                TreatmentPlaceCount = _treatmentPlaceService.Count(),
                AttendanceTypeCount = _attendanceTypeService.Count(),
                AttendantCount = _attendanceTypeService.Count(),
                AuxiliarAcessoryTypeCount = _auxiliarAccessoryTypeService.Count(),
                BenefitCount = _patientBenefitService.Count(),
                BenefitTypeCount = _benefitService.Count(),
                ExpenseTypeCount = _expenseTypeService.Count(),
                ServiceTypeCount = _serviceTypeService.Count(),
                StaysCount = _stayService.Count(),
                VisitorCount = _visitorService.Count(),
                VisitorAttendanceTypeCount = _visitorAttendanceTypeTypeService.Count(),
            };

            return View(homeView);
        }
    }
}
