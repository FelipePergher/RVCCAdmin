using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Data.Models.PatientModels;
using RVCC.Data.Models.RelationModels;
using RVCC.Data.Repositories;
using RVCC.Models.FormModel;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminAndUserAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class PatientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataRepository<Patient> _patientService;
        private readonly IDataRepository<Doctor> _doctorService;
        private readonly IDataRepository<TreatmentPlace> _treatmentPlaceService;
        private readonly IDataRepository<CancerType> _cancerTypeService;
        private readonly IDataRepository<Medicine> _medicineService;
        private readonly IDataRepository<Naturality> _naturalityService;
        private readonly IDataRepository<PatientInformation> _patientInformationService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(
            IDataRepository<Patient> patientService,
            IDataRepository<Doctor> doctorService,
            IDataRepository<TreatmentPlace> treatmentPlaceService,
            IDataRepository<CancerType> cancerTypeService,
            IDataRepository<Medicine> medicineService,
            IDataRepository<Naturality> naturalityService,
            IDataRepository<PatientInformation> patientInformationService,
            ILogger<PatientController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _patientService = patientService;
            _userManager = userManager;
            _doctorService = doctorService;
            _cancerTypeService = cancerTypeService;
            _treatmentPlaceService = treatmentPlaceService;
            _medicineService = medicineService;
            _naturalityService = naturalityService;
            _patientInformationService = patientInformationService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new PatientSearchModel());
        }

        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            Patient patient = await _patientService.FindByIdAsync(id, new[]
            {
                "PatientInformation", "Naturality",
                "PatientInformation.PatientInformationCancerTypes", "PatientInformation.PatientInformationCancerTypes.CancerType", 
                "PatientInformation.PatientInformationDoctors", "PatientInformation.PatientInformationDoctors.Doctor", 
                "PatientInformation.PatientInformationMedicines", "PatientInformation.PatientInformationMedicines.Medicine", 
                "PatientInformation.PatientInformationTreatmentPlaces", "PatientInformation.PatientInformationTreatmentPlaces.TreatmentPlace", 
            });

            var patientDetails = new PatientDetailsViewModel
            {
                PatientId = id,
                NaturalityId = patient.Naturality.NaturalityId.ToString(),
                PatientInformationId = patient.PatientInformation.PatientInformationId.ToString(),
                PatientProfile = new PatientProfileFormModel
                {
                    FirstName = patient.FirstName,
                    Surname = patient.Surname,
                    RG = patient.RG,
                    CPF = patient.CPF,
                    Profession = patient.Profession,
                    Sex = patient.Sex,
                    CivilState = patient.CivilState,
                    DateOfBirth = patient.DateOfBirth.ToShortDateString(),
                    JoinDate = patient.JoinDate.ToShortDateString(),
                    FamiliarityGroup = patient.FamiliarityGroup,
                    MonthlyIncome = patient.MonthlyIncome.ToString()
                },
                PatientInformation = new PatientInformationFormModel
                {
                    CancerTypes = patient.PatientInformation.PatientInformationCancerTypes.Select(x => x.CancerType.Name).ToList(),
                    Doctors = patient.PatientInformation.PatientInformationDoctors.Select(x => x.Doctor.Name).ToList(),
                    Medicines = patient.PatientInformation.PatientInformationMedicines.Select(x => x.Medicine.Name).ToList(),
                    TreatmentPlaces = patient.PatientInformation.PatientInformationTreatmentPlaces.Select(x => x.TreatmentPlace.City).ToList(),
                    TreatmentBeginDate = patient.PatientInformation.TreatmentBeginDate.ToShortDateString()
                },
                Naturality = new NaturalityFormModel
                {
                    City = patient.Naturality.City,
                    Country = patient.Naturality.Country,
                    State = patient.Naturality.State
                }
            };
            return View(patientDetails);
        }

        #region Add Methods

        [HttpGet]
        public IActionResult AddPatientProfile()
        {
            return PartialView("Partials/_AddPatientProfile", new PatientProfileFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientProfile(PatientProfileFormModel patientProfileForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                var patient = new Patient
                {
                    FirstName = patientProfileForm.FirstName,
                    Surname = patientProfileForm.Surname,
                    RG = patientProfileForm.RG,
                    CPF = patientProfileForm.CPF,
                    FamiliarityGroup = patientProfileForm.FamiliarityGroup,
                    Sex = patientProfileForm.Sex,
                    CivilState = patientProfileForm.CivilState,
                    DateOfBirth = DateTime.Parse(patientProfileForm.DateOfBirth),
                    JoinDate = DateTime.Parse(patientProfileForm.JoinDate),
                    Profession = patientProfileForm.Profession,
                    CreatedBy = user.Name,
                    MonthlyIncome =
                        (double)(decimal.TryParse(patientProfileForm.MonthlyIncome, out decimal monthlyIncome) ? monthlyIncome : 0)
                };

                TaskResult result = await _patientService.CreateAsync(patient);

                if (result.Succeeded)
                {
                    return Ok(new { ok = true, url = Url.Action("AddPatientNaturality", new { id = patient.Naturality.NaturalityId }), title = "Adicionar Naturalidade" });
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_AddPatientProfile", patientProfileForm);
        }

        [HttpGet]
        public IActionResult AddPatientNaturality(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            return PartialView("Partials/_AddPatientNaturality", new NaturalityFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientNaturality(string id, NaturalityFormModel naturalityForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Naturality naturality = await _naturalityService.FindByIdAsync(id, new[] { "Patient", "Patient.PatientInformation" });

                naturality.City = naturalityForm.City;
                naturality.State = naturalityForm.State;
                naturality.Country = naturalityForm.Country;
                naturality.UpdatedBy = user.Name;

                TaskResult result = await _naturalityService.UpdateAsync(naturality);

                if (result.Succeeded)
                {
                    Patient patient = await _patientService.FindByIdAsync(naturality.PatientId.ToString(), new[] { "PatientInformation" });

                    return Ok(new
                    {
                        ok = true,
                        url = Url.Action("AddPatientInformation",
                    new { id = patient.PatientInformation.PatientInformationId }),
                        title = "Adicionar Informação do Paciente"
                    });
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_AddPatientNaturality", naturalityForm);
        }

        [HttpGet]
        public async Task<IActionResult> AddPatientInformation(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var patientInformationForm = new PatientInformationFormModel()
            {
                SelectDoctors = await SelectHelper.GetDoctorSelectAsync(_doctorService),
                SelectCancerTypes = await SelectHelper.GetCancerTypesSelectAsync(_cancerTypeService),
                SelectMedicines = await SelectHelper.GetMedicinesSelectAsync(_medicineService),
                SelectTreatmentPlaces = await SelectHelper.GetTreatmentPlaceSelectAsync(_treatmentPlaceService)
            };
            return PartialView("Partials/_AddPatientInformation", patientInformationForm);
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientInformation(string id, PatientInformationFormModel patientInformationForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                PatientInformation patientInformation = await _patientInformationService.FindByIdAsync(id,
                    new[] { "PatientInformationCancerTypes", "PatientInformationDoctors", "PatientInformationMedicines", "PatientInformationTreatmentPlaces" });

                patientInformation.TreatmentBeginDate = !string.IsNullOrEmpty(patientInformationForm.TreatmentBeginDate)
                    ? DateTime.Parse(patientInformationForm.TreatmentBeginDate)
                    : DateTime.MinValue;

                //Added Cancer Types to Patient Information
                foreach (string cancerTypeValue in patientInformationForm.CancerTypes)
                {
                    bool isCancerTypeInt = int.TryParse(cancerTypeValue, out int num);

                    CancerType cancerType = null;
                    if (isCancerTypeInt)
                    {
                        cancerType = await _cancerTypeService.FindByIdAsync(cancerTypeValue);
                    }

                    if (cancerType == null)
                    {
                        await ((CancerTypeRepository)_cancerTypeService).FindByNameAsync(cancerTypeValue);
                    }

                    if (cancerType == null)
                    {
                        cancerType = new CancerType(cancerTypeValue, user);
                    }

                    patientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType(cancerType));
                }

                //Added Doctor to Patient Information
                foreach (string doctorValue in patientInformationForm.Doctors)
                {
                    Doctor doctor = null;
                    bool isDoctorTypeInt = int.TryParse(doctorValue, out int num);
                    if (isDoctorTypeInt)
                    {
                        doctor = await _doctorService.FindByIdAsync(doctorValue);
                    }

                    if (doctor == null)
                    {
                        doctor = await ((DoctorRepository)_doctorService).FindByNameAsync(doctorValue);
                    }

                    if (doctor == null)
                    {
                        doctor = new Doctor(doctorValue, user);
                    }

                    patientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor(doctor));
                }

                //Added Treatment Place to Patient Information
                foreach (string treatmentPlaceValue in patientInformationForm.TreatmentPlaces)
                {
                    TreatmentPlace treatmentPlace = int.TryParse(treatmentPlaceValue, out int num) ?
                        await _treatmentPlaceService.FindByIdAsync(treatmentPlaceValue) : await ((TreatmentPlaceRepository)_treatmentPlaceService).FindByCityAsync(treatmentPlaceValue);
                    if (treatmentPlace == null)
                    {
                        treatmentPlace = new TreatmentPlace(treatmentPlaceValue, user);
                    }

                    patientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace(treatmentPlace));
                }

                //Added Medicine to Patient Information
                foreach (string item in patientInformationForm.Medicines)
                {
                    Medicine medicine = int.TryParse(item, out int num) ?
                        await _medicineService.FindByIdAsync(item) : await ((MedicineRepository)_medicineService).FindByNameAsync(item);

                    if (medicine == null)
                    {
                        medicine = new Medicine(item, user);
                    }

                    patientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine(medicine));
                }

                TaskResult result = await _patientInformationService.UpdateAsync(patientInformation);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            patientInformationForm.SelectDoctors = await SelectHelper.GetDoctorSelectAsync(_doctorService);
            patientInformationForm.SelectCancerTypes = await SelectHelper.GetCancerTypesSelectAsync(_cancerTypeService);
            patientInformationForm.SelectMedicines = await SelectHelper.GetMedicinesSelectAsync(_medicineService);
            patientInformationForm.SelectTreatmentPlaces = await SelectHelper.GetTreatmentPlaceSelectAsync(_treatmentPlaceService);

            return PartialView("Partials/_AddPatientInformation", patientInformationForm);
        }

        #endregion

        #region Edit Methods

        [HttpGet]
        public async Task<IActionResult> EditPatientProfile(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Patient patient = await _patientService.FindByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            var patientProfileForm = new PatientProfileFormModel
            {
                PatientId = patient.PatientId,
                FirstName = patient.FirstName,
                Surname = patient.Surname,
                RG = patient.RG,
                CPF = patient.CPF,
                FamiliarityGroup = patient.FamiliarityGroup,
                Sex = patient.Sex,
                CivilState = patient.CivilState,
                DateOfBirth = patient.DateOfBirth.ToString("dd/MM/yyyy"),
                JoinDate = patient.JoinDate.ToString("dd/MM/yyyy"),
                MonthlyIncome = patient.MonthlyIncome.ToString("C2"),
                Profession = patient.Profession
            };

            return PartialView("Partials/_EditPatientProfile", patientProfileForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientProfile(string id, PatientProfileFormModel patientProfileForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Patient patient = await _patientService.FindByIdAsync(id);

                patient.FirstName = patientProfileForm.FirstName;
                patient.Surname = patientProfileForm.Surname;
                patient.RG = patientProfileForm.RG;
                patient.CPF = patientProfileForm.CPF;
                patient.FamiliarityGroup = patientProfileForm.FamiliarityGroup;
                patient.Sex = patientProfileForm.Sex;
                patient.CivilState = patientProfileForm.CivilState;
                patient.DateOfBirth = DateTime.Parse(patientProfileForm.DateOfBirth);
                patient.JoinDate = DateTime.Parse(patientProfileForm.JoinDate);
                patient.Profession = patientProfileForm.Profession;
                patient.UpdatedBy = user.Name;
                patient.MonthlyIncome =
                        (double)(decimal.TryParse(patientProfileForm.MonthlyIncome, out decimal monthlyIncome) ? monthlyIncome : 0);

                TaskResult result = await _patientService.UpdateAsync(patient);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_EditPatientProfile", patientProfileForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditPatientNaturality(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Naturality naturality = await _naturalityService.FindByIdAsync(id);

            if (naturality == null)
            {
                return NotFound();
            }

            var naturalityForm = new NaturalityFormModel
            {
                City = naturality.City,
                Country = naturality.Country,
                State = naturality.State
            };

            return PartialView("Partials/_EditPatientNaturality", naturalityForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientNaturality(string id, NaturalityFormModel naturalityForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                Naturality naturality = await _naturalityService.FindByIdAsync(id);

                naturality.City = naturalityForm.City;
                naturality.State = naturalityForm.State;
                naturality.Country = naturalityForm.Country;
                naturality.UpdatedBy = user.Name;

                TaskResult result = await _naturalityService.UpdateAsync(naturality);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_EditPatientNaturality", naturalityForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditPatientInformation(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            PatientInformation patientInformation = await _patientInformationService.FindByIdAsync(id,
                new[] { "PatientInformationCancerTypes", "PatientInformationDoctors", "PatientInformationMedicines", "PatientInformationTreatmentPlaces" });

            if (patientInformation == null)
            {
                return NotFound();
            }

            var patientInformationForm = new PatientInformationFormModel
            {
                SelectDoctors = await SelectHelper.GetDoctorSelectAsync(_doctorService),
                SelectCancerTypes = await SelectHelper.GetCancerTypesSelectAsync(_cancerTypeService),
                SelectMedicines = await SelectHelper.GetMedicinesSelectAsync(_medicineService),
                SelectTreatmentPlaces = await SelectHelper.GetTreatmentPlaceSelectAsync(_treatmentPlaceService),
                CancerTypes = patientInformation.PatientInformationCancerTypes.Select(x => x.CancerType.CancerTypeId.ToString()).ToList(),
                Doctors = patientInformation.PatientInformationDoctors.Select(x => x.Doctor.DoctorId.ToString()).ToList(),
                Medicines = patientInformation.PatientInformationMedicines.Select(x => x.Medicine.MedicineId.ToString()).ToList(),
                TreatmentPlaces = patientInformation.PatientInformationTreatmentPlaces.Select(x => x.TreatmentPlace.TreatmentPlaceId.ToString()).ToList()
            };
            if (patientInformation.TreatmentBeginDate != DateTime.MinValue)
            {
                patientInformationForm.TreatmentBeginDate = patientInformation.TreatmentBeginDate.ToString("dd/MM/yyyy");
            }

            return PartialView("Partials/_EditPatientInformation", patientInformationForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientInformation(string id, PatientInformationFormModel patientInformationForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                PatientInformation patientInformation = await _patientInformationService.FindByIdAsync(id,
                    new[] { "PatientInformationCancerTypes", "PatientInformationDoctors", "PatientInformationMedicines", "PatientInformationTreatmentPlaces" });

                patientInformation.TreatmentBeginDate = !string.IsNullOrEmpty(patientInformationForm.TreatmentBeginDate)
                   ? DateTime.Parse(patientInformationForm.TreatmentBeginDate)
                   : DateTime.MinValue;

                //Added Cancer Types to Patient Information
                if (patientInformationForm.CancerTypes.Count == 0)
                {
                    patientInformation.PatientInformationCancerTypes.Clear();
                }
                else
                {
                    //Filter to stay only selecteds that already stay on db
                    patientInformation.PatientInformationCancerTypes =
                        patientInformation.PatientInformationCancerTypes.Where(x => patientInformationForm.CancerTypes.Contains(x.CancerTypeId.ToString())).ToList();

                    //Filter to stay on selected only that no stay on db yet
                    patientInformationForm.CancerTypes =
                        patientInformationForm.CancerTypes.Where(x => !patientInformation.PatientInformationCancerTypes.Any(y => y.CancerTypeId.ToString() == x)).ToList();

                    //Add news cancer types
                    foreach (string cancerTypeValue in patientInformationForm.CancerTypes)
                    {
                        CancerType cancerType = null;
                        bool isCancerTypeInt = int.TryParse(cancerTypeValue, out int num);
                        if (isCancerTypeInt)
                        {
                            cancerType = await _cancerTypeService.FindByIdAsync(cancerTypeValue);
                        }

                        if (cancerType == null)
                        {
                            cancerType = await ((CancerTypeRepository)_cancerTypeService).FindByNameAsync(cancerTypeValue);
                        }

                        if (cancerType == null)
                        {
                            cancerType = new CancerType(cancerTypeValue, user);
                        }

                        patientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType(cancerType));
                    }
                }

                //Added Doctor to Patient Information
                if (patientInformationForm.Doctors.Count == 0)
                {
                    patientInformation.PatientInformationDoctors.Clear();
                }
                else
                {
                    //Filter to stay only selecteds that already stay on db
                    patientInformation.PatientInformationDoctors =
                        patientInformation.PatientInformationDoctors.Where(x => patientInformationForm.Doctors.Contains(x.DoctorId.ToString())).ToList();

                    //Filter to stay on selected only that no stay on db yet
                    patientInformationForm.Doctors =
                        patientInformationForm.Doctors.Where(x => !patientInformation.PatientInformationDoctors.Any(y => y.DoctorId.ToString() == x)).ToList();

                    //Add news Doctors
                    foreach (string doctorValue in patientInformationForm.Doctors)
                    {
                        Doctor doctor = null;
                        bool isDoctorTypeInt = int.TryParse(doctorValue, out int num);
                        if (isDoctorTypeInt)
                        {
                            doctor = await _doctorService.FindByIdAsync(doctorValue);
                        }

                        if (doctor == null)
                        {
                            doctor = await ((DoctorRepository)_doctorService).FindByNameAsync(doctorValue);
                        }

                        if (doctor == null)
                        {
                            doctor = new Doctor(doctorValue, user);
                        }

                        patientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor(doctor));
                    }
                }

                //Added Treatment Places to Patient Information
                if (patientInformationForm.TreatmentPlaces.Count == 0)
                {
                    patientInformation.PatientInformationTreatmentPlaces.Clear();
                }
                else
                {
                    //Filter to stay only selecteds that already stay on db
                    patientInformation.PatientInformationTreatmentPlaces =
                        patientInformation.PatientInformationTreatmentPlaces.Where(x => patientInformationForm.TreatmentPlaces.Contains(x.TreatmentPlaceId.ToString())).ToList();

                    //Filter to stay on selected only that no stay on db yet
                    patientInformationForm.TreatmentPlaces =
                        patientInformationForm.TreatmentPlaces.Where(x => !patientInformation.PatientInformationTreatmentPlaces.Any(y => y.TreatmentPlaceId.ToString() == x)).ToList();

                    //Add news TreatmentPlaces
                    foreach (string treatmentPlaceValue in patientInformationForm.TreatmentPlaces)
                    {
                        TreatmentPlace treatmentPlace = null;
                        bool isTreatmentPlaceTypeInt = int.TryParse(treatmentPlaceValue, out int num);
                        if (isTreatmentPlaceTypeInt)
                        {
                            treatmentPlace = await _treatmentPlaceService.FindByIdAsync(treatmentPlaceValue);
                        }

                        if (treatmentPlace == null)
                        {
                            treatmentPlace = await ((TreatmentPlaceRepository)_treatmentPlaceService).FindByCityAsync(treatmentPlaceValue);
                        }

                        if (treatmentPlace == null)
                        {
                            treatmentPlace = new TreatmentPlace(treatmentPlaceValue, user);
                        }

                        patientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace(treatmentPlace));
                    }
                }

                //Added Medicine to Patient Information
                if (patientInformationForm.Medicines.Count == 0)
                {
                    patientInformation.PatientInformationMedicines.Clear();
                }
                else
                {
                    //Filter to stay only selecteds that already stay on db
                    patientInformation.PatientInformationMedicines =
                        patientInformation.PatientInformationMedicines.Where(x => patientInformationForm.Medicines.Contains(x.MedicineId.ToString())).ToList();

                    //Filter to stay on selected only that no stay on db yet
                    patientInformationForm.Medicines =
                        patientInformationForm.Medicines.Where(x => !patientInformation.PatientInformationMedicines.Any(y => y.MedicineId.ToString() == x)).ToList();

                    //Add news Doctors
                    foreach (string medicineValue in patientInformationForm.Medicines)
                    {
                        Medicine medicine = null;
                        bool isMedicineTypeInt = int.TryParse(medicineValue, out int num);
                        if (isMedicineTypeInt)
                        {
                            medicine = await _medicineService.FindByIdAsync(medicineValue);
                        }

                        if (medicine == null)
                        {
                            medicine = await ((MedicineRepository)_medicineService).FindByNameAsync(medicineValue);
                        }

                        if (medicine == null)
                        {
                            medicine = new Medicine(medicineValue, user);
                        }

                        patientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine(medicine));
                    }
                }

                TaskResult result = await _patientInformationService.UpdateAsync(patientInformation);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            patientInformationForm.SelectDoctors = await SelectHelper.GetDoctorSelectAsync(_doctorService);
            patientInformationForm.SelectCancerTypes = await SelectHelper.GetCancerTypesSelectAsync(_cancerTypeService);
            patientInformationForm.SelectMedicines = await SelectHelper.GetMedicinesSelectAsync(_medicineService);
            patientInformationForm.SelectTreatmentPlaces = await SelectHelper.GetTreatmentPlaceSelectAsync(_treatmentPlaceService);

            return PartialView("Partials/_EditPatientInformation", patientInformationForm);
        }

        #endregion

        #region Control Enable/Disable Methods

        [HttpGet]
        public IActionResult ArchivePatient(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            return PartialView("Partials/_ArchivePatient", new ArchivePatientFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> ArchivePatient(string id, ArchivePatientFormModel archivePatientForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Patient patient = await _patientService.FindByIdAsync(id, new[] { "ActivePatient", "PatientInformation" });

            if (patient == null)
            {
                return NotFound();
            }

            var dateTime = DateTime.Parse(archivePatientForm.DateTime);
            switch (archivePatientForm.ArchivePatientType)
            {
                case Globals.ArchivePatientType.death:
                    patient.ActivePatient.Death = true;
                    patient.ActivePatient.DeathDate = dateTime;
                    break;
                case Globals.ArchivePatientType.discharge:
                    patient.ActivePatient.Discharge = true;
                    patient.ActivePatient.DischargeDate = dateTime;
                    break;
            }

            TaskResult result = await _patientService.UpdateAsync(patient);

            if (result.Succeeded)
            {
                return Ok();
            }

            return PartialView("Partials/_ArchivePatient", archivePatientForm);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> ActivePatient(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Patient patient = await _patientService.FindByIdAsync(id, new[] { "PatientInformation", "ActivePatient" });

            if (patient == null)
            {
                return NotFound();
            }

            patient.ActivePatient.Discharge = false;
            TaskResult result = await _patientService.UpdateAsync(patient);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeletePatient(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Patient patient = await _patientService.FindByIdAsync(id);

            if (patient == null)
            {
                return NotFound();
            }

            TaskResult result = await _patientService.DeleteAsync(patient);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest();
        }

        #endregion
    }
}