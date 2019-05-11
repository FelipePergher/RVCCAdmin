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
using System;
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
            return View(new PatientSearchModel());
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
                    MonthlyIncome = patientProfileForm.MonthlyIncome ?? 0
                };

                TaskResult result = await _patientService.CreateAsync(patient);

                if (result.Succeeded) return Ok(new { ok = true, url = Url.Action("AddPatientNaturality", new { id = patient.Naturality.NaturalityId }), title = "Adicionar Naturalidade" });
                return BadRequest(result.Errors);
            }

            return PartialView("Partials/_AddPatientProfile", patientProfileForm);
        }

        [HttpGet]
        public IActionResult AddPatientNaturality(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            return PartialView("Partials/_AddPatientNaturality", new NaturalityFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientNaturality(string id, NaturalityFormModel naturalityForm)
        {
            if (ModelState.IsValid)
            {
                BaseSpecification<Naturality> baseSpecification = new BaseSpecification<Naturality>(x => x.Patient, x => x.Patient.PatientInformation);
                Naturality naturality = await _naturalityService.FindByIdAsync(id, baseSpecification);

                naturality.City = naturalityForm.City;
                naturality.State = naturalityForm.State;
                naturality.Country = naturalityForm.Country;
                naturality.UserUpdated = await _userManager.GetUserAsync(User);

                TaskResult result = await _naturalityService.UpdateAsync(naturality);

                if (result.Succeeded) return Ok(new { ok = true, url = Url.Action("AddPatientInformation", 
                    new { id = naturality.Patient.PatientInformation.PatientInformationId }), title = "Adicionar Informação do Paciente" });
                return BadRequest(result.Errors);
            }

            return PartialView("Partials/_AddPatientNaturality", naturalityForm);
        }

        [HttpGet]
        public async Task<IActionResult> AddPatientInformation(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

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

                BaseSpecification<PatientInformation> specification = new BaseSpecification<PatientInformation>(
                    x => x.PatientInformationCancerTypes, x => x.PatientInformationDoctors, x => x.PatientInformationMedicines, x => x.PatientInformationTreatmentPlaces);

                PatientInformation patientInformation = await _patientInformationService.FindByIdAsync(id, specification);

                //Added Cancer Types to Patient Information
                foreach (string cancerTypeValue in patientInformationForm.CancerTypes)
                {
                    bool isCancerTypeInt = int.TryParse(cancerTypeValue, out int num);

                    CancerType cancerType = null;
                    if (isCancerTypeInt) cancerType = await _cancerTypeService.FindByIdAsync(cancerTypeValue);
                    if (cancerType == null) await ((CancerTypeStore)_cancerTypeService).FindByNameAsync(cancerTypeValue);

                    if (cancerType == null) cancerType = new CancerType(cancerTypeValue, user);

                    patientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType(cancerType));
                }

                //Added Doctor to Patient Information
                foreach (string doctorValue in patientInformationForm.Doctors)
                {
                    Doctor doctor = null;
                    bool isDoctorTypeInt = int.TryParse(doctorValue, out int num);
                    if (isDoctorTypeInt) doctor = await _doctorService.FindByIdAsync(doctorValue);
                    if (doctor == null) doctor = await ((DoctorStore)_doctorService).FindByNameAsync(doctorValue);
                    if (doctor == null) doctor = new Doctor(doctorValue, user);

                    patientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor(doctor));
                }

                //Added Treatment Place to Patient Information
                foreach (string treatmentPlaceValue in patientInformationForm.TreatmentPlaces)
                {
                    TreatmentPlace treatmentPlace = int.TryParse(treatmentPlaceValue, out int num) ?
                        await _treatmentPlaceService.FindByIdAsync(treatmentPlaceValue) : await ((TreatmentPlaceStore)_treatmentPlaceService).FindByCityAsync(treatmentPlaceValue);
                    if (treatmentPlace == null) treatmentPlace = new TreatmentPlace(treatmentPlaceValue, user);
                    patientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace(treatmentPlace));
                }

                //Added Medicine to Patient Information
                foreach (string item in patientInformationForm.Medicines)
                {
                    Medicine medicine = int.TryParse(item, out int num) ?
                        await _medicineService.FindByIdAsync(item) : await ((MedicineStore)_medicineService).FindByNameAsync(item);

                    if (medicine == null) medicine = new Medicine(item, user);
                    patientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine(medicine));
                }

                TaskResult result = await _patientInformationService.UpdateAsync(patientInformation);

                if (result.Succeeded) return Ok(new { ok = true, url = Url.Action("AddPatientPhone", new { id = patientInformation.PatientId }), title = "Adicionar Telefone" });
                return BadRequest(result.Errors);
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
            if (string.IsNullOrEmpty(id)) return BadRequest();

            Patient patient = await _patientService.FindByIdAsync(id);

            if(patient == null) return NotFound();

            PatientProfileFormModel patientProfileForm = new PatientProfileFormModel
            {
                PatientId = id,
                FirstName = patient.FirstName,
                Surname = patient.Surname,
                RG = patient.RG,
                CPF = patient.CPF,
                FamiliarityGroup = patient.FamiliarityGroup,
                Sex = patient.Sex,
                CivilState = patient.CivilState,
                DateOfBirth = patient.DateOfBirth,
                MonthlyIncome = patient.MonthlyIncome,
                Profession = patient.Profession
            };

            return PartialView("Partials/_EditPatientProfile", patientProfileForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientProfile(string id, PatientProfileFormModel patientProfileForm)
        {
            if (ModelState.IsValid)
            {
                Patient patient = await _patientService.FindByIdAsync(id);

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
                patient.MonthlyIncome = patientProfileForm.MonthlyIncome ?? 0;

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
                Naturality naturality = await _naturalityService.FindByIdAsync(id);

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
            return PartialView("Partials/_EditPatientInformation", patientInformationForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientInformation(string id, PatientInformationFormModel patientInformationForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                BaseSpecification<PatientInformation> specification = new BaseSpecification<PatientInformation>(
                    x => x.PatientInformationCancerTypes, x => x.PatientInformationDoctors, x => x.PatientInformationMedicines, x => x.PatientInformationTreatmentPlaces);

                PatientInformation patientInformation = await _patientInformationService.FindByIdAsync(id, specification);

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

            patientInformationForm.SelectDoctors = await SelectHelper.GetDoctorSelectAsync(_doctorService);
            patientInformationForm.SelectCancerTypes = await SelectHelper.GetCancerTypesSelectAsync(_cancerTypeService);
            patientInformationForm.SelectMedicines = await SelectHelper.GetMedicinesSelectAsync(_medicineService);
            patientInformationForm.SelectTreatmentPlaces = await SelectHelper.GetTreatmentPlaceSelectAsync(_treatmentPlaceService);

            return PartialView("Partials/_AddPatientInformation", patientInformationForm);
        }

        #endregion

        #region Control Enable/Disable Methods

        [HttpGet]
        public IActionResult ArchivePatient(string id)
        {
            ArchivePatientFormModel archivePatientForm = new ArchivePatientFormModel
            {
                DateTime = DateTime.Now
            };
            return PartialView("Partials/_ArchivePatient", archivePatientForm);
        }

        [HttpPost]
        public async Task<IActionResult> ArchivePatient(string id, ArchivePatientFormModel archivePatientForm)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.PatientInformation, x => x.ActivePatient);
            Patient patient = await _patientService.FindByIdAsync(id, specification);
            if (patient == null) return NotFound();

            switch (archivePatientForm.ArchivePatientType)
            {
                case Globals.ArchivePatientType.death:
                    patient.ActivePatient.Death = true;
                    patient.ActivePatient.DeathDate = archivePatientForm.DateTime;
                    break;
                case Globals.ArchivePatientType.discharge:
                    patient.ActivePatient.Discharge = true;
                    patient.ActivePatient.DischargeDate = archivePatientForm.DateTime;
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

            BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.PatientInformation, x => x.ActivePatient);
            Patient patient = await _patientService.FindByIdAsync(id, specification);

            if (patient == null) return NotFound();

            patient.ActivePatient.Discharge = false;
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

        public async Task<IActionResult> IsCpfExist(string cpf, int patientId)
        {
            Patient patient = await ((PatientStore)_patientService).FindByCpfAsync(cpf, patientId);
            return Ok(patient == null);
        }

        public async Task<IActionResult> IsRgExist(string rg, int patientId)
        {
            Patient patient = await ((PatientStore)_patientService).FindByRgAsync(rg, patientId);
            return Ok(patient == null);
        }

        #endregion
    }
}