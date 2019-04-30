using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Models.RelationModels;
using LigaCancer.Data.Store;
using LigaCancer.Models.FormModel;
using LigaCancer.Models.SearchModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin"), AutoValidateAntiforgeryToken]
    public class PatientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Patient> _patientService;
        private readonly IDataStore<Doctor> _doctorService;
        private readonly IDataStore<TreatmentPlace> _treatmentPlaceService;
        private readonly IDataStore<CancerType> _cancerTypeService;
        private readonly IDataStore<Medicine> _medicineService;
        private readonly IDataStore<Naturality> _naturalityService;
        private readonly IDataStore<PatientInformation> _patientInformationService;

        public PatientController(
            IDataStore<Patient> patientService,
            IDataStore<Doctor> doctorService,
            IDataStore<TreatmentPlace> treatmentPlaceService,
            IDataStore<CancerType> cancerTypeService,
            IDataStore<Medicine> medicineService,
            IDataStore<Naturality> naturalityService,
            IDataStore<PatientInformation> patientInformationService,
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
        }

        [HttpGet]
        public IActionResult Index()
        {
            PatientSearchModel patientSearch = new PatientSearchModel
            {
                SelectCancerTypes = _cancerTypeService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.CancerTypeId.ToString()
                }).ToList(),
                SelectDoctors = _doctorService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.DoctorId.ToString()
                }).ToList(),
                SelectMedicines = _medicineService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.MedicineId.ToString()
                }).ToList(),
                SelectTreatmentPlaces = _treatmentPlaceService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.City,
                    Value = x.TreatmentPlaceId.ToString()
                }).ToList()
            };
            return View(patientSearch);
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
                Patient patient = new Patient
                {
                    FirstName = patientProfileForm.FirstName,
                    Surname = patientProfileForm.Surname,
                    RG = patientProfileForm.RG,
                    CPF = patientProfileForm.CPF,
                    FamiliarityGroup = patientProfileForm.FamiliarityGroup,
                    Sex = patientProfileForm.Sex,
                    CivilState = patientProfileForm.CivilState,
                    DateOfBirth = patientProfileForm.DateOfBirth.Value,
                    Profession = patientProfileForm.Profession,
                    UserCreated = await _userManager.GetUserAsync(User),
                    Family = new Family
                    {
                        MonthlyIncome = patientProfileForm.MonthlyIncome ?? 0
                    }
                };

                TaskResult result = await _patientService.CreateAsync(patient);

                if (result.Succeeded) return Ok(new { ok = true, url = Url.Action("AddPatientNaturality", new { id = result.Id }), title = "Adicionar Naturalidade" });
                return BadRequest(result.Errors);
            }

            return PartialView("Partials/_AddPatientProfile", patientProfileForm);
        }

        [HttpGet]
        public IActionResult AddPatientNaturality(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            return PartialView("Partials/_AddPatientNaturality", new NaturalityFormModel(id));
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientNaturality(NaturalityFormModel naturalityForm)
        {
            if (ModelState.IsValid)
            {
                BaseSpecification<Patient> baseSpecification = new BaseSpecification<Patient>(x => x.Naturality);
                Patient patient = await _patientService.FindByIdAsync(naturalityForm.PatientId, baseSpecification);

                patient.Naturality.PatientId = int.Parse(naturalityForm.PatientId);
                patient.Naturality.City = naturalityForm.City;
                patient.Naturality.State = naturalityForm.State;
                patient.Naturality.Country = naturalityForm.Country;
                patient.Naturality.UserCreated = await _userManager.GetUserAsync(User);

                TaskResult result = await _naturalityService.UpdateAsync(patient.Naturality);

                if (result.Succeeded) return Ok(new { ok = true, url = Url.Action("AddPatientInformation", new { id = patient.PatientId }), title = "Adicionar Informação do Paciente" });
                return BadRequest(result.Errors);
            }

            return PartialView("Partials/_AddPatientNaturality", naturalityForm);
        }

        [HttpGet]
        public IActionResult AddPatientInformation(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var patientInformationForm = new PatientInformationFormModel(id)
            {
                SelectDoctors = _doctorService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.DoctorId.ToString()
                }).ToList(),
                SelectCancerTypes = _cancerTypeService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.CancerTypeId.ToString()
                }).ToList(),
                SelectMedicines = _medicineService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.MedicineId.ToString()
                }).ToList(),
                SelectTreatmentPlaces = _treatmentPlaceService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.City,
                    Value = x.TreatmentPlaceId.ToString()
                }).ToList()
            };
            return PartialView("Partials/_AddPatientInformation", patientInformationForm);
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientInformation(PatientInformationFormModel patientInformationForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                Patient patient = await _patientService.FindByIdAsync(patientInformationForm.PatientId);
                patient.PatientInformation = new PatientInformation();

                //Added Cancer Types to Patient Information
                foreach (string cancerTypeValue in patientInformationForm.CancerTypes)
                {
                    bool isCancerTypeInt = int.TryParse(cancerTypeValue, out int num);

                    CancerType cancerType = null;
                    if (isCancerTypeInt) cancerType = await _cancerTypeService.FindByIdAsync(cancerTypeValue);
                    if (cancerType == null) await ((CancerTypeStore)_cancerTypeService).FindByNameAsync(cancerTypeValue);

                    if (cancerType == null) cancerType = new CancerType(cancerTypeValue, user);

                    patient.PatientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType(cancerType));
                }

                //Added Doctor to Patient Information
                foreach (string doctorValue in patientInformationForm.Doctors)
                {
                    Doctor doctor = null;
                    bool isDoctorTypeInt = int.TryParse(doctorValue, out int num);
                    if (isDoctorTypeInt) doctor = await _doctorService.FindByIdAsync(doctorValue);
                    if (doctor == null) doctor = await ((DoctorStore)_doctorService).FindByNameAsync(doctorValue);
                    if (doctor == null) doctor = new Doctor(doctorValue, user);

                    patient.PatientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor(doctor));
                }

                //Added Treatment Place to Patient Information
                foreach (string treatmentPlaceValue in patientInformationForm.TreatmentPlaces)
                {
                    TreatmentPlace treatmentPlace = int.TryParse(treatmentPlaceValue, out int num) ?
                        await _treatmentPlaceService.FindByIdAsync(treatmentPlaceValue) : await ((TreatmentPlaceStore)_treatmentPlaceService).FindByCityAsync(treatmentPlaceValue);
                    if (treatmentPlace == null)
                    {
                        treatmentPlace = new TreatmentPlace(treatmentPlaceValue, user);
                    }
                    patient.PatientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace(treatmentPlace));
                }

                //Added Medicine to Patient Information
                foreach (string item in patientInformationForm.Medicines)
                {
                    Medicine medicine = int.TryParse(item, out int num) ?
                        await _medicineService.FindByIdAsync(item) : await ((MedicineStore)_medicineService).FindByNameAsync(item);

                    if (medicine == null)
                    {
                        medicine = new Medicine(item, user);
                    }
                    patient.PatientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine(medicine));
                }

                TaskResult result = await _patientService.UpdateAsync(patient);

                if (result.Succeeded) return Ok(new { ok = true, url = Url.Action("AddPatientPhone", new { id = patient.PatientId }), title = "Adicionar Telefone" });
                return BadRequest(result.Errors);
            }

            patientInformationForm.SelectDoctors = _doctorService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.DoctorId.ToString()
            }).ToList();
            patientInformationForm.SelectCancerTypes = _cancerTypeService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CancerTypeId.ToString()
            }).ToList();
            patientInformationForm.SelectMedicines = _medicineService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.MedicineId.ToString()
            }).ToList();
            patientInformationForm.SelectTreatmentPlaces = _treatmentPlaceService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.TreatmentPlaceId.ToString()
            }).ToList();

            return PartialView("Partials/_AddPatientInformation", patientInformationForm);
        }

        #endregion

        #region Edit Methods

        [HttpGet]
        public async Task<IActionResult> EditPatientProfile(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            BaseSpecification<Patient> baseSpecification = new BaseSpecification<Patient>(x => x.Family);
            Patient patient = await _patientService.FindByIdAsync(id, baseSpecification);

            if(patient == null) return NotFound();

            PatientProfileFormModel patientProfileForm = new PatientProfileFormModel
            {
                PatientId = patient.PatientId.ToString(),
                FirstName = patient.FirstName,
                Surname = patient.Surname,
                RG = patient.RG,
                CPF = patient.CPF,
                FamiliarityGroup = patient.FamiliarityGroup,
                Sex = patient.Sex,
                CivilState = patient.CivilState,
                DateOfBirth = patient.DateOfBirth,
                MonthlyIncome = patient.Family.MonthlyIncome,
                Profession = patient.Profession
            };

            return PartialView("Partials/_EditPatientProfile", patientProfileForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientProfile(PatientProfileFormModel patientProfileForm)
        {
            if (ModelState.IsValid)
            {
                BaseSpecification<Patient> baseSpecification = new BaseSpecification<Patient>(x => x.Family);
                Patient patient = await _patientService.FindByIdAsync(patientProfileForm.PatientId, baseSpecification);

                patient.FirstName = patientProfileForm.FirstName;
                patient.Surname = patientProfileForm.Surname;
                patient.RG = patientProfileForm.RG;
                patient.CPF = patientProfileForm.CPF;
                patient.FamiliarityGroup = patientProfileForm.FamiliarityGroup;
                patient.Sex = patientProfileForm.Sex;
                patient.CivilState = patientProfileForm.CivilState;
                patient.DateOfBirth = patientProfileForm.DateOfBirth.Value;
                patient.Profession = patientProfileForm.Profession;
                patient.UserUpdated = await _userManager.GetUserAsync(User);
                patient.Family.MonthlyIncome = patientProfileForm.MonthlyIncome ?? 0;

                TaskResult result = await _patientService.UpdateAsync(patient);

                if (result.Succeeded) return Ok();
                return BadRequest(result.Errors);
            }

            return PartialView("Partials/_EditPatientProfile", patientProfileForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditPatientNaturality(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Naturality naturality = await _naturalityService.FindByIdAsync(id);

            if(naturality == null) return NotFound();

            NaturalityFormModel naturalityForm = new NaturalityFormModel
            {
                NaturalityId = id,
                City = naturality.City,
                Country = naturality.Country,
                State = naturality.State
            };

            return PartialView("Partials/_EditPatientNaturality", naturalityForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientNaturality(NaturalityFormModel naturalityForm)
        {
            if (ModelState.IsValid)
            {
                Naturality naturality = await _naturalityService.FindByIdAsync(naturalityForm.NaturalityId);

                naturality.City = naturalityForm.City;
                naturality.State = naturalityForm.State;
                naturality.Country = naturalityForm.Country;
                naturality.UserUpdated = await _userManager.GetUserAsync(User);

                TaskResult result = await _naturalityService.UpdateAsync(naturality);

                if (result.Succeeded) return Ok();
                return BadRequest(result.Errors);
            }

            return PartialView("Partials/_EditPatientNaturality", naturalityForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditPatientInformation(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            BaseSpecification<PatientInformation> specification = new BaseSpecification<PatientInformation>(
                x => x.PatientInformationCancerTypes, x => x.PatientInformationDoctors, x => x.PatientInformationMedicines, x => x.PatientInformationTreatmentPlaces);

            PatientInformation patientInformation = await _patientInformationService.FindByIdAsync(id, specification);

            if (patientInformation == null) return NotFound();

            var patientInformationForm = new PatientInformationFormModel(id)
            {
                PatientInformationId = id,
                SelectDoctors = _doctorService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.DoctorId.ToString()
                }).ToList(),
                SelectCancerTypes = _cancerTypeService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.CancerTypeId.ToString()
                }).ToList(),
                SelectMedicines = _medicineService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.MedicineId.ToString()
                }).ToList(),
                SelectTreatmentPlaces = _treatmentPlaceService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.City,
                    Value = x.TreatmentPlaceId.ToString()
                }).ToList(),

                CancerTypes = patientInformation.PatientInformationCancerTypes.Select(x => x.CancerType.CancerTypeId.ToString()).ToList(),
                Doctors = patientInformation.PatientInformationDoctors.Select(x => x.Doctor.DoctorId.ToString()).ToList(),
                Medicines = patientInformation.PatientInformationMedicines.Select(x => x.Medicine.MedicineId.ToString()).ToList(),
                TreatmentPlaces = patientInformation.PatientInformationTreatmentPlaces.Select(x => x.TreatmentPlace.TreatmentPlaceId.ToString()).ToList()
            };
            return PartialView("Partials/_EditPatientInformation", patientInformationForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientInformation(PatientInformationFormModel patientInformationForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                BaseSpecification<PatientInformation> specification = new BaseSpecification<PatientInformation>(
                    x => x.PatientInformationCancerTypes, x => x.PatientInformationDoctors, x => x.PatientInformationMedicines, x => x.PatientInformationTreatmentPlaces);

                PatientInformation patientInformation = await _patientInformationService.FindByIdAsync(patientInformationForm.PatientInformationId, specification);

                //Added Cancer Types to Patient Information
                if (patientInformationForm.CancerTypes.Count == 0) patientInformation.PatientInformationCancerTypes.Clear();
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
                        if (isCancerTypeInt) cancerType = await _cancerTypeService.FindByIdAsync(cancerTypeValue);
                        if (cancerType == null) cancerType = await ((CancerTypeStore)_cancerTypeService).FindByNameAsync(cancerTypeValue);
                        if (cancerType == null) cancerType = new CancerType(cancerTypeValue, user);

                        patientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType(cancerType));
                    }
                }

                //Added Doctor to Patient Information
                if (patientInformationForm.Doctors.Count == 0) patientInformation.PatientInformationDoctors.Clear();
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
                        if (isDoctorTypeInt) doctor = await _doctorService.FindByIdAsync(doctorValue);
                        if (doctor == null) doctor = await ((DoctorStore)_doctorService).FindByNameAsync(doctorValue);
                        if (doctor == null) doctor = new Doctor(doctorValue, user);

                        patientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor(doctor));
                    }
                }

                //Added Treatment Places to Patient Information
                if (patientInformationForm.TreatmentPlaces.Count == 0) patientInformation.PatientInformationTreatmentPlaces.Clear();
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
                        if (isTreatmentPlaceTypeInt) treatmentPlace = await _treatmentPlaceService.FindByIdAsync(treatmentPlaceValue);
                        if (treatmentPlace == null) treatmentPlace = await ((TreatmentPlaceStore)_treatmentPlaceService).FindByCityAsync(treatmentPlaceValue);
                        if (treatmentPlace == null) treatmentPlace = new TreatmentPlace(treatmentPlaceValue, user);

                        patientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace(treatmentPlace));
                    }
                }

                //Added Medicine to Patient Information
                if (patientInformationForm.Medicines.Count == 0) patientInformation.PatientInformationMedicines.Clear();
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
                        if (isMedicineTypeInt) medicine = await _medicineService.FindByIdAsync(medicineValue);
                        if (medicine == null) medicine = await ((MedicineStore)_medicineService).FindByNameAsync(medicineValue);
                        if (medicine == null) medicine = new Medicine(medicineValue, user);

                        patientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine(medicine));
                    }
                }

                TaskResult result = await _patientInformationService.UpdateAsync(patientInformation);

                if (result.Succeeded) return Ok();
                return BadRequest(result.Errors);
            }

            patientInformationForm.SelectDoctors = _doctorService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.DoctorId.ToString()
            }).ToList();
            patientInformationForm.SelectCancerTypes = _cancerTypeService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CancerTypeId.ToString()
            }).ToList();
            patientInformationForm.SelectMedicines = _medicineService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.MedicineId.ToString()
            }).ToList();
            patientInformationForm.SelectTreatmentPlaces = _treatmentPlaceService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.TreatmentPlaceId.ToString()
            }).ToList();

            return PartialView("Partials/_AddPatientInformation", patientInformationForm);
        }

        #endregion

        #region Control Enable/Disable Methods

        [HttpGet]
        public IActionResult ArchivePatient(string id)
        {
            ArchivePatientFormModel archivePatientForm = new ArchivePatientFormModel
            {
                PatientId = id,
                DateTime = DateTime.Now
            };
            return PartialView("Partials/_ArchivePatient", archivePatientForm);
        }

        [HttpPost]
        public async Task<IActionResult> ArchivePatient(ArchivePatientFormModel archivePatientForm)
        {
            if (string.IsNullOrEmpty(archivePatientForm.PatientId)) return BadRequest();

            BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.PatientInformation, x => x.PatientInformation.ActivePatient);
            Patient patient = await _patientService.FindByIdAsync(archivePatientForm.PatientId, specification);
            if (patient == null) return NotFound();

            switch (archivePatientForm.ArchivePatientType)
            {
                case Globals.ArchivePatientType.death:
                    patient.PatientInformation.ActivePatient.Death = true;
                    patient.PatientInformation.ActivePatient.DeathDate = archivePatientForm.DateTime;
                    break;
                case Globals.ArchivePatientType.discharge:
                    patient.PatientInformation.ActivePatient.Discharge = true;
                    patient.PatientInformation.ActivePatient.DischargeDate = archivePatientForm.DateTime;
                    break;
            }

            TaskResult result = await _patientService.UpdateAsync(patient);

            if (result.Succeeded) return Ok();

            return PartialView("Partials/_ArchivePatient", archivePatientForm);
        }

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> ActivePatient(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.PatientInformation, x => x.PatientInformation.ActivePatient);
            Patient patient = await _patientService.FindByIdAsync(id, specification);

            if (patient == null) return NotFound();

            patient.PatientInformation.ActivePatient.Discharge = false;
            TaskResult result = await _patientService.UpdateAsync(patient);

            if (result.Succeeded) return Ok();

            return BadRequest();
        }

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeletePatient(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Patient patient = await _patientService.FindByIdAsync(id);

            if (patient == null) return NotFound();

            TaskResult result = await _patientService.DeleteAsync(patient);

            if (result.Succeeded) return Ok();

            return BadRequest();
        }

        #endregion

        #region Custom Methods

        public JsonResult IsCpfExist(string cpf, int patientId)
        {
            Patient patient = ((PatientStore)_patientService).FindByCpfAsync(cpf, patientId).Result;

            return patient != null ? Json(false) : Json(true);
        }

        public JsonResult IsRgExist(string rg, int patientId)
        {
            Patient patient = ((PatientStore)_patientService).FindByRgAsync(rg, patientId).Result;

            return Json(patient == null);
        }

        #endregion

        #region Remove this

        //[HttpGet]
        //public IActionResult AddPatient()
        //{
        //    PatientFormModel patientForm = new PatientFormModel
        //    {
        //        SelectDoctors = _doctorService.GetAllAsync().Result.Select(x => new SelectListItem
        //        {
        //            Text = x.Name,
        //            Value = x.DoctorId.ToString()
        //        }).ToList(),
        //        SelectCancerTypes = _cancerTypeService.GetAllAsync().Result.Select(x => new SelectListItem
        //        {
        //            Text = x.Name,
        //            Value = x.CancerTypeId.ToString()
        //        }).ToList(),
        //        SelectMedicines = _medicineService.GetAllAsync().Result.Select(x => new SelectListItem
        //        {
        //            Text = x.Name,
        //            Value = x.MedicineId.ToString()
        //        }).ToList(),
        //        SelectTreatmentPlaces = _treatmentPlaceService.GetAllAsync().Result.Select(x => new SelectListItem
        //        {
        //            Text = x.City,
        //            Value = x.TreatmentPlaceId.ToString()
        //        }).ToList(),
        //        DateOfBirth = DateTime.Now
        //    };

        //    return PartialView("Partials/_AddPatient", patientForm);
        //}

        //[HttpPost]
        //public async Task<IActionResult> AddPatient(PatientFormModel patientForm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        ApplicationUser user = await _userManager.GetUserAsync(User);

        //        Patient patient = new Patient
        //        {
        //            FirstName = patientForm.FirstName,
        //            Surname = patientForm.Surname,
        //            RG = patientForm.RG,
        //            CPF = patientForm.CPF,
        //            FamiliarityGroup = patientForm.FamiliarityGroup,
        //            Sex = patientForm.Sex,
        //            CivilState = patientForm.CivilState,
        //            DateOfBirth = patientForm.DateOfBirth,
        //            Profession = patientForm.Profession,
        //            Naturality = new Naturality
        //            {
        //                City = patientForm.Naturality.City,
        //                State = patientForm.Naturality.State,
        //                Country = patientForm.Naturality.Country,
        //                UserCreated = user,
        //            },
        //            UserCreated = user,
        //            Family = new Family
        //            {
        //                MonthlyIncome = patientForm.MonthlyIncome ?? 0,
        //                //FamilyIncome = patientForm.MonthlyIncome != null ? (double)patientForm.MonthlyIncome : 0,
        //                //PerCapitaIncome = patientForm.MonthlyIncome != null ? (double)patientForm.MonthlyIncome : 0
        //            }
        //        };

        //        //Added Cancer Types to Patient Information
        //        foreach (string item in patientForm.PatientInformation.CancerTypes)
        //        {
        //            CancerType cancerType = int.TryParse(item, out int num) ? await _cancerTypeService.FindByIdAsync(item) : await ((CancerTypeStore)_cancerTypeService).FindByNameAsync(item);
        //            if (cancerType == null)
        //            {
        //                cancerType = new CancerType(item, user);
        //                TaskResult result = await _cancerTypeService.CreateAsync(cancerType);
        //                if (result.Succeeded)
        //                {
        //                    patient.PatientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType(cancerType));
        //                }
        //            }
        //            else
        //            {
        //                patient.PatientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType(cancerType));
        //            }

        //        }

        //        //Added Doctor to Patient Information
        //        foreach (string item in patientForm.PatientInformation.Doctors)
        //        {
        //            Doctor doctor = int.TryParse(item, out int num) ? await _doctorService.FindByIdAsync(item) : await ((DoctorStore)_doctorService).FindByNameAsync(item);
        //            if (doctor == null)
        //            {
        //                doctor = new Doctor
        //                {
        //                    Name = item,
        //                    UserCreated = user
        //                };
        //                TaskResult result = await _doctorService.CreateAsync(doctor);
        //                if (result.Succeeded)
        //                {
        //                    patient.PatientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor
        //                    {
        //                        Doctor = doctor
        //                    });
        //                }
        //            }
        //            else
        //            {
        //                patient.PatientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor
        //                {
        //                    Doctor = doctor
        //                });
        //            }
        //        }

        //        //Added Treatment Place to Patient Information
        //        foreach (string item in patientForm.PatientInformation.TreatmentPlaces)
        //        {
        //            TreatmentPlace treatmentPlace = int.TryParse(item, out int num) ?
        //                await _treatmentPlaceService.FindByIdAsync(item) : await ((TreatmentPlaceStore)_treatmentPlaceService).FindByCityAsync(item);
        //            if (treatmentPlace == null)
        //            {
        //                treatmentPlace = new TreatmentPlace
        //                {
        //                    City = item,
        //                    UserCreated = user
        //                };
        //                TaskResult result = await _treatmentPlaceService.CreateAsync(treatmentPlace);
        //                if (result.Succeeded)
        //                {
        //                    patient.PatientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace
        //                    {
        //                        TreatmentPlace = treatmentPlace
        //                    });
        //                }
        //            }
        //            else
        //            {
        //                patient.PatientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace
        //                {
        //                    TreatmentPlace = treatmentPlace
        //                });
        //            }

        //        }

        //        //Added Medicine to Patient Information
        //        foreach (string item in patientForm.PatientInformation.Medicines)
        //        {
        //            Medicine medicine = int.TryParse(item, out int num) ?
        //                await _medicineService.FindByIdAsync(item) : await ((MedicineStore)_medicineService).FindByNameAsync(item);

        //            if (medicine == null)
        //            {
        //                medicine = new Medicine
        //                {
        //                    Name = item,
        //                    UserCreated = user
        //                };
        //                TaskResult result = await _medicineService.CreateAsync(medicine);
        //                if (result.Succeeded)
        //                {
        //                    patient.PatientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine
        //                    {
        //                        Medicine = medicine
        //                    });
        //                }
        //            }
        //            else
        //            {
        //                patient.PatientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine
        //                {
        //                    Medicine = medicine
        //                });
        //            }

        //        }

        //        TaskResult result = await _patientService.CreateAsync(patient);
        //        if (result.Succeeded) return Ok();
        //        ModelState.AddErrors(result);
        //    }

        //    patientForm.SelectDoctors = _doctorService.GetAllAsync().Result.Select(x => new SelectListItem
        //    {
        //        Text = x.Name,
        //        Value = x.DoctorId.ToString()
        //    }).ToList();

        //    patientForm.SelectCancerTypes = _cancerTypeService.GetAllAsync().Result.Select(x => new SelectListItem
        //    {
        //        Text = x.Name,
        //        Value = x.CancerTypeId.ToString()
        //    }).ToList();

        //    patientForm.SelectMedicines = _medicineService.GetAllAsync().Result.Select(x => new SelectListItem
        //    {
        //        Text = x.Name,
        //        Value = x.MedicineId.ToString()
        //    }).ToList();

        //    patientForm.SelectTreatmentPlaces = _treatmentPlaceService.GetAllAsync().Result.Select(x => new SelectListItem
        //    {
        //        Text = x.City,
        //        Value = x.TreatmentPlaceId.ToString()
        //    }).ToList();
        //    return PartialView("Partials/_AddPatient", patientForm);
        //}

        //public async Task<IActionResult> DetailsPatient(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //    {
        //        return BadRequest();
        //    }

        //    BaseSpecification<Patient> specification = new BaseSpecification<Patient>(
        //           x => x.Naturality, x => x.Family, x => x.FileAttachments, x => x.PatientInformation,
        //           x => x.PatientInformation.PatientInformationCancerTypes,
        //           x => x.PatientInformation.PatientInformationDoctors,
        //           x => x.PatientInformation.PatientInformationMedicines,
        //           x => x.PatientInformation.PatientInformationTreatmentPlaces
        //       );
        //    specification.IncludeStrings.Add("PatientInformation.PatientInformationDoctors.Doctor");
        //    specification.IncludeStrings.Add("PatientInformation.PatientInformationMedicines.Medicine");
        //    specification.IncludeStrings.Add("PatientInformation.PatientInformationCancerTypes.CancerType");
        //    specification.IncludeStrings.Add("PatientInformation.PatientInformationTreatmentPlaces.TreatmentPlace");

        //    Patient patient = await _patientService.FindByIdAsync(id, specification);

        //    if (patient == null)
        //    {
        //        return NotFound();
        //    }

        //    PatientShowFormModel patientShowForm = new PatientShowFormModel
        //    {
        //        PatientId = patient.PatientId,
        //        Name = patient.FirstName + " " + patient.Surname,
        //        CivilState = patient.CivilState,
        //        CPF = patient.CPF,
        //        DateOfBirth = patient.DateOfBirth,
        //        FamiliarityGroup = patient.FamiliarityGroup,
        //        Profession = patient.Profession,
        //        RG = patient.RG,
        //        Sex = patient.Sex,
        //        Family = patient.Family,
        //        //Naturality = new NaturalityFormModel
        //        //{
        //        //    City = patient.Naturality.City,
        //        //    Country = patient.Naturality.Country,
        //        //    State = patient.Naturality.State
        //        //},
        //        FileAttachments = patient.FileAttachments,
        //        //PatientInformation = new PatientInformationFormModel
        //        //{
        //        //    CancerTypes = patient.PatientInformation.PatientInformationCancerTypes.Select(x => x.CancerType.Name).ToList(),
        //        //    Doctors = patient.PatientInformation.PatientInformationDoctors.Select(x => x.Doctor.Name).ToList(),
        //        //    Medicines = patient.PatientInformation.PatientInformationMedicines.Select(x => x.Medicine.Name).ToList(),
        //        //    TreatmentPlaces = patient.PatientInformation.PatientInformationTreatmentPlaces.Select(x => x.TreatmentPlace.City).ToList(),
        //        //}
        //    };

        //    return View(patientShowForm);
        //}

        //[HttpGet]
        //public async Task<IActionResult> EditPatient(string id)
        //{
        //    if (string.IsNullOrEmpty(id)) return BadRequest();

        //    BaseSpecification<Patient> specification = new BaseSpecification<Patient>(
        //        x => x.Naturality, x => x.PatientInformation, x => x.Family,
        //        x => x.PatientInformation.PatientInformationCancerTypes,
        //        x => x.PatientInformation.PatientInformationDoctors,
        //        x => x.PatientInformation.PatientInformationMedicines,
        //        x => x.PatientInformation.PatientInformationTreatmentPlaces
        //    );
        //    Patient patient = await _patientService.FindByIdAsync(id, specification);

        //    if (patient == null) return BadRequest();

        //    PatientFormModel patientForm = new PatientFormModel
        //    {
        //        SelectDoctors = _doctorService.GetAllAsync().Result.Select(x => new SelectListItem
        //        {
        //            Text = x.Name,
        //            Value = x.DoctorId.ToString()
        //        }).ToList(),
        //        SelectCancerTypes = _cancerTypeService.GetAllAsync().Result.Select(x => new SelectListItem
        //        {
        //            Text = x.Name,
        //            Value = x.CancerTypeId.ToString()
        //        }).ToList(),
        //        SelectMedicines = _medicineService.GetAllAsync().Result.Select(x => new SelectListItem
        //        {
        //            Text = x.Name,
        //            Value = x.MedicineId.ToString()
        //        }).ToList(),
        //        SelectTreatmentPlaces = _treatmentPlaceService.GetAllAsync().Result.Select(x => new SelectListItem
        //        {
        //            Text = x.City,
        //            Value = x.TreatmentPlaceId.ToString()
        //        }).ToList(),
        //        CivilState = patient.CivilState,
        //        CPF = patient.CPF,
        //        DateOfBirth = patient.DateOfBirth,
        //        FamiliarityGroup = patient.FamiliarityGroup,
        //        FirstName = patient.FirstName,
        //        //Naturality = new NaturalityFormModel
        //        //{
        //        //    City = patient.Naturality.City,
        //        //    Country = patient.Naturality.Country,
        //        //    State = patient.Naturality.State
        //        //},
        //        PatientId = patient.PatientId,
        //        //PatientInformation = new PatientInformationFormModel
        //        //{
        //        //    CancerTypes = patient.PatientInformation.PatientInformationCancerTypes.Select(x => x.CancerType.CancerTypeId.ToString()).ToList(),
        //        //    Doctors = patient.PatientInformation.PatientInformationDoctors.Select(x => x.Doctor.DoctorId.ToString()).ToList(),
        //        //    Medicines = patient.PatientInformation.PatientInformationMedicines.Select(x => x.Medicine.MedicineId.ToString()).ToList(),
        //        //    TreatmentPlaces = patient.PatientInformation.PatientInformationTreatmentPlaces.Select(x => x.TreatmentPlace.TreatmentPlaceId.ToString()).ToList()
        //        //},
        //        Profession = patient.Profession,
        //        RG = patient.RG,
        //        Sex = patient.Sex,
        //        Surname = patient.Surname,
        //        MonthlyIncome = patient.Family.MonthlyIncome
        //    };

        //    return PartialView("Partials/_EditPatient", patientForm);
        //}

        //[HttpPost]
        //public async Task<IActionResult> EditPatient(string id, PatientFormModel patientForm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        BaseSpecification<Patient> specification = new BaseSpecification<Patient>(
        //            x => x.Naturality, x => x.PatientInformation, x => x.Family, x => x.Family.FamilyMembers,
        //            x => x.PatientInformation.PatientInformationCancerTypes,
        //            x => x.PatientInformation.PatientInformationDoctors,
        //            x => x.PatientInformation.PatientInformationMedicines,
        //            x => x.PatientInformation.PatientInformationTreatmentPlaces
        //        );
        //        specification.IncludeStrings.Add("PatientInformation.PatientInformationDoctors.Doctor");
        //        specification.IncludeStrings.Add("PatientInformation.PatientInformationMedicines.Medicine");
        //        specification.IncludeStrings.Add("PatientInformation.PatientInformationCancerTypes.CancerType");
        //        specification.IncludeStrings.Add("PatientInformation.PatientInformationTreatmentPlaces.TreatmentPlace");

        //        Patient patient = await _patientService.FindByIdAsync(id, specification);
        //        ApplicationUser user = await _userManager.GetUserAsync(User);


        //        //Added Cancer Types to Patient Information
        //        if (patientForm.PatientInformation.CancerTypes.Count == 0)
        //        {
        //            patient.PatientInformation.PatientInformationCancerTypes.Clear();
        //        }
        //        else
        //        {
        //            //Remove not selected of database
        //            List<PatientInformationCancerType> removePatientInformationCancerTypes = new List<PatientInformationCancerType>();
        //            foreach (PatientInformationCancerType item in patient.PatientInformation.PatientInformationCancerTypes)
        //            {
        //                if (patientForm.PatientInformation.CancerTypes.FirstOrDefault(x => x == item.CancerType.Name) == null)
        //                    removePatientInformationCancerTypes.Add(item);
        //            }
        //            patient.PatientInformation.PatientInformationCancerTypes = patient.PatientInformation.PatientInformationCancerTypes.Except(removePatientInformationCancerTypes).ToList();

        //            //Add news cancer types
        //            foreach (string item in patientForm.PatientInformation.CancerTypes)
        //            {
        //                CancerType cancerType = int.TryParse(item, out int num)
        //                    ? await _cancerTypeService.FindByIdAsync(item)
        //                    : await ((CancerTypeStore)_cancerTypeService).FindByNameAsync(item);
        //                //if (cancerType == null)
        //                //{
        //                //    cancerType = new CancerType
        //                //    {
        //                //        Name = item,
        //                //        UserCreated = user
        //                //    };
        //                //}
        //                //if (patient.PatientInformation.PatientInformationCancerTypes.FirstOrDefault(x => x.CancerTypeId == cancerType.CancerTypeId) == null)
        //                //{
        //                //    patient.PatientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType
        //                //    {
        //                //        CancerType = cancerType
        //                //    });
        //                //}
        //            }
        //        }

        //        //Added Doctor to Patient Information
        //        if (patientForm.PatientInformation.Doctors.Count == 0)
        //        {
        //            patient.PatientInformation.PatientInformationDoctors.Clear();
        //        }
        //        else
        //        {
        //            //Remove not selected of database
        //            List<PatientInformationDoctor> removePatientInformationDoctors = new List<PatientInformationDoctor>();
        //            foreach (PatientInformationDoctor item in patient.PatientInformation.PatientInformationDoctors)
        //            {
        //                if (patientForm.PatientInformation.Doctors.FirstOrDefault(x => x == item.Doctor.Name) == null)
        //                {
        //                    removePatientInformationDoctors.Add(item);
        //                }
        //            }
        //            patient.PatientInformation.PatientInformationDoctors = patient.PatientInformation.PatientInformationDoctors.Except(removePatientInformationDoctors).ToList();

        //            //Add news doctors
        //            foreach (string item in patientForm.PatientInformation.Doctors)
        //            {
        //                Doctor doctor = int.TryParse(item, out int num) ? await _doctorService.FindByIdAsync(item) : await ((DoctorStore)_doctorService).FindByNameAsync(item);
        //                if (doctor == null)
        //                {
        //                    doctor = new Doctor
        //                    {
        //                        Name = item,
        //                        UserCreated = user
        //                    };
        //                }
        //                if (patient.PatientInformation.PatientInformationDoctors.FirstOrDefault(x => x.DoctorId == doctor.DoctorId) == null)
        //                {
        //                    patient.PatientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor
        //                    {
        //                        Doctor = doctor
        //                    });
        //                }
        //            }

        //        }

        //        //Added Treatment Place to Patient Information
        //        if (patientForm.PatientInformation.TreatmentPlaces.Count == 0) patient.PatientInformation.PatientInformationTreatmentPlaces.Clear();
        //        else
        //        {
        //            //Remove not selected of database
        //            List<PatientInformationTreatmentPlace> removePatientInformationTreatmentPlaces = new List<PatientInformationTreatmentPlace>();
        //            foreach (PatientInformationTreatmentPlace item in patient.PatientInformation.PatientInformationTreatmentPlaces)
        //            {
        //                if (patientForm.PatientInformation.TreatmentPlaces.FirstOrDefault(x => x == item.TreatmentPlace.City) == null)
        //                    removePatientInformationTreatmentPlaces.Add(item);
        //            }
        //            patient.PatientInformation.PatientInformationTreatmentPlaces = patient.PatientInformation.PatientInformationTreatmentPlaces.Except(removePatientInformationTreatmentPlaces).ToList();

        //            //Add new Treatment Place
        //            foreach (string item in patientForm.PatientInformation.TreatmentPlaces)
        //            {
        //                TreatmentPlace treatmentPlace = int.TryParse(item, out int num) ?
        //                    await _treatmentPlaceService.FindByIdAsync(item) : await ((TreatmentPlaceStore)_treatmentPlaceService).FindByCityAsync(item);

        //                if (treatmentPlace == null)
        //                {
        //                    treatmentPlace = new TreatmentPlace
        //                    {
        //                        City = item,
        //                        UserCreated = user
        //                    };
        //                }

        //                if (patient.PatientInformation.PatientInformationTreatmentPlaces.FirstOrDefault(x => x.TreatmentPlaceId == treatmentPlace.TreatmentPlaceId) == null)
        //                {
        //                    patient.PatientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace
        //                    {
        //                        TreatmentPlace = treatmentPlace
        //                    });
        //                }
        //            }
        //        }

        //        //Added Medicine to Patient Information
        //        if (patientForm.PatientInformation.Medicines.Count == 0)
        //        {
        //            patient.PatientInformation.PatientInformationMedicines.Clear();
        //        }
        //        else
        //        {

        //            //Remove not selected of database
        //            List<PatientInformationMedicine> removePatientInformationMedicines = new List<PatientInformationMedicine>();
        //            foreach (PatientInformationMedicine item in patient.PatientInformation.PatientInformationMedicines)
        //            {
        //                if (patientForm.PatientInformation.TreatmentPlaces.FirstOrDefault(x => x == item.Medicine.Name) == null)
        //                {
        //                    removePatientInformationMedicines.Add(item);
        //                }
        //            }
        //            patient.PatientInformation.PatientInformationMedicines = patient.PatientInformation.PatientInformationMedicines.Except(removePatientInformationMedicines).ToList();

        //            //Added Medicine to Patient Information
        //            foreach (var item in patientForm.PatientInformation.Medicines)
        //            {
        //                Medicine medicine = int.TryParse(item, out int num) ?
        //                    await _medicineService.FindByIdAsync(item) : await ((MedicineStore)_medicineService).FindByNameAsync(item);

        //                if (medicine == null)
        //                {
        //                    medicine = new Medicine
        //                    {
        //                        Name = item,
        //                        UserCreated = user
        //                    };
        //                }

        //                if (patient.PatientInformation.PatientInformationMedicines.FirstOrDefault(x => x.MedicineId == medicine.MedicineId) == null)
        //                {
        //                    patient.PatientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine
        //                    {
        //                        Medicine = medicine
        //                    });
        //                }
        //            }
        //        }

        //        patient.UpdatedDate = DateTime.Now;
        //        patient.UserUpdated = user;

        //        patient.FirstName = patientForm.FirstName;
        //        patient.Surname = patientForm.Surname;
        //        patient.RG = patientForm.RG;
        //        patient.CPF = patientForm.CPF;
        //        patient.FamiliarityGroup = patientForm.FamiliarityGroup;
        //        patient.Sex = patientForm.Sex;
        //        patient.CivilState = patientForm.CivilState;
        //        patient.DateOfBirth = patientForm.DateOfBirth;
        //        patient.Naturality.City = patientForm.Naturality.City;
        //        patient.Naturality.State = patientForm.Naturality.State;
        //        patient.Naturality.Country = patientForm.Naturality.Country;
        //        patient.Profession = patientForm.Profession;

        //        //patient.Family.FamilyIncome -= (double)patient.Family.MonthlyIncome;
        //        //patient.Family.FamilyIncome += (double)patientForm.MonthlyIncome;
        //        patient.Family.MonthlyIncome = patientForm.MonthlyIncome ?? 0;

        //        //patient.Family.PerCapitaIncome = patient.Family.FamilyIncome / (patient.Family.FamilyMembers.Count + 1);

        //        TaskResult result = await _patientService.UpdateAsync(patient);
        //        if (result.Succeeded) return Ok();
        //        ModelState.AddErrors(result);
        //    }

        //    patientForm.SelectDoctors = _doctorService.GetAllAsync().Result.Select(x => new SelectListItem
        //    {
        //        Text = x.Name,
        //        Value = x.DoctorId.ToString()
        //    }).ToList();

        //    patientForm.SelectCancerTypes = _cancerTypeService.GetAllAsync().Result.Select(x => new SelectListItem
        //    {
        //        Text = x.Name,
        //        Value = x.CancerTypeId.ToString()
        //    }).ToList();

        //    patientForm.SelectMedicines = _medicineService.GetAllAsync().Result.Select(x => new SelectListItem
        //    {
        //        Text = x.Name,
        //        Value = x.MedicineId.ToString()
        //    }).ToList();

        //    patientForm.SelectTreatmentPlaces = _treatmentPlaceService.GetAllAsync().Result.Select(x => new SelectListItem
        //    {
        //        Text = x.City,
        //        Value = x.TreatmentPlaceId.ToString()
        //    }).ToList();

        //    return PartialView("Partials/_EditPatient", patientForm);
        //}

        #endregion
    }
}