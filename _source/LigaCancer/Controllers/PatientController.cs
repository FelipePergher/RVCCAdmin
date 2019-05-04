﻿using LigaCancer.Code;
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
        public async Task<IActionResult> Index()
        {
            PatientSearchModel patientSearch = new PatientSearchModel
            {
                SelectDoctors = await SelectHelper.GetDoctorSelectAsync(_doctorService),
                SelectCancerTypes = await SelectHelper.GetCancerTypesSelectAsync(_cancerTypeService),
                SelectMedicines = await SelectHelper.GetMedicinesSelectAsync(_medicineService),
                SelectTreatmentPlaces = await SelectHelper.GetTreatmentPlaceSelectAsync(_treatmentPlaceService)
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
        public async Task<IActionResult> AddPatientInformation(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            var patientInformationForm = new PatientInformationFormModel(id)
            {
                SelectDoctors = await SelectHelper.GetDoctorSelectAsync(_doctorService),
                SelectCancerTypes = await SelectHelper.GetCancerTypesSelectAsync(_cancerTypeService),
                SelectMedicines = await SelectHelper.GetMedicinesSelectAsync(_medicineService),
                SelectTreatmentPlaces = await SelectHelper.GetTreatmentPlaceSelectAsync(_treatmentPlaceService)
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

        //[HttpPost]
        //public async Task<IActionResult> AddPatient(PatientFormModel patientForm)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        Patient patient = new Patient
        //        {
        //            Family = new Family
        //            {
        //                MonthlyIncome = patientForm.MonthlyIncome ?? 0,
        //                //FamilyIncome = patientForm.MonthlyIncome != null ? (double)patientForm.MonthlyIncome : 0,
        //                //PerCapitaIncome = patientForm.MonthlyIncome != null ? (double)patientForm.MonthlyIncome : 0
        //            }
        //        };
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

        //        //patient.Family.FamilyIncome -= (double)patient.Family.MonthlyIncome;
        //        //patient.Family.FamilyIncome += (double)patientForm.MonthlyIncome;
        //        patient.Family.MonthlyIncome = patientForm.MonthlyIncome ?? 0;

        //        //patient.Family.PerCapitaIncome = patient.Family.FamilyIncome / (patient.Family.FamilyMembers.Count + 1);

        //        TaskResult result = await _patientService.UpdateAsync(patient);
        //        if (result.Succeeded) return Ok();
        //        ModelState.AddErrors(result);
        //    }

        //}

        #endregion
    }
}