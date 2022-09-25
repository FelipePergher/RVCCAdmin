// <copyright file="PatientController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
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
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class PatientController : Controller
    {
        private readonly IDataRepository<Patient> _patientService;
        private readonly IDataRepository<Doctor> _doctorService;
        private readonly IDataRepository<TreatmentPlace> _treatmentPlaceService;
        private readonly IDataRepository<CancerType> _cancerTypeService;
        private readonly IDataRepository<Medicine> _medicineService;
        private readonly IDataRepository<ServiceType> _serviceTypeService;
        private readonly IDataRepository<Naturality> _naturalityService;
        private readonly IDataRepository<PatientInformation> _patientInformationService;
        private readonly ILogger<PatientController> _logger;

        public PatientController(
            IDataRepository<Patient> patientService,
            IDataRepository<Doctor> doctorService,
            IDataRepository<TreatmentPlace> treatmentPlaceService,
            IDataRepository<CancerType> cancerTypeService,
            IDataRepository<Medicine> medicineService,
            IDataRepository<ServiceType> serviceTypeService,
            IDataRepository<Naturality> naturalityService,
            IDataRepository<PatientInformation> patientInformationService,
            ILogger<PatientController> logger)
        {
            _patientService = patientService;
            _doctorService = doctorService;
            _treatmentPlaceService = treatmentPlaceService;
            _cancerTypeService = cancerTypeService;
            _serviceTypeService = serviceTypeService;
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
            string[] includes =
            {
                nameof(Patient.PatientInformation), nameof(Patient.Naturality),
                $"{nameof(Patient.PatientInformation)}.{nameof(PatientInformation.PatientInformationDoctors)}.{nameof(PatientInformationDoctor.Doctor)}",
                $"{nameof(Patient.PatientInformation)}.{nameof(PatientInformation.PatientInformationCancerTypes)}.{nameof(PatientInformationCancerType.CancerType)}",
                $"{nameof(Patient.PatientInformation)}.{nameof(PatientInformation.PatientInformationMedicines)}.{nameof(PatientInformationMedicine.Medicine)}",
                $"{nameof(Patient.PatientInformation)}.{nameof(PatientInformation.PatientInformationTreatmentPlaces)}.{nameof(PatientInformationTreatmentPlace.TreatmentPlace)}",
                $"{nameof(Patient.PatientInformation)}.{nameof(PatientInformation.PatientInformationServiceTypes)}.{nameof(PatientInformationServiceType.ServiceType)}",
            };

            Patient patient = await _patientService.FindByIdAsync(id, includes);

            if (patient == null)
            {
                return NotFound();
            }

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
                    ForwardedToSupportHouse = patient.ForwardedToSupportHouse,
                    MonthlyIncome = patient.MonthlyIncome.ToString("C2"),
                    ImmediateNecessities = patient.ImmediateNecessities,
                    ImmediateNecessitiesDateUpdated = patient.ImmediateNecessitiesDateUpdated
                },
                PatientInformation = new PatientInformationFormModel
                {
                    CancerTypes = patient.PatientInformation.PatientInformationCancerTypes.Select(x => x.CancerType.Name).ToList(),
                    Doctors = patient.PatientInformation.PatientInformationDoctors.Select(x => x.Doctor.Name).ToList(),
                    Medicines = patient.PatientInformation.PatientInformationMedicines.Select(x => x.Medicine.Name).ToList(),
                    TreatmentPlaces = patient.PatientInformation.PatientInformationTreatmentPlaces.Select(x => x.TreatmentPlace.City).ToList(),
                    ServiceTypes = patient.PatientInformation.PatientInformationServiceTypes.Select(x => x.ServiceType.Name).ToList(),
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

        [HttpGet]
        public async Task<IActionResult> Print(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            string[] includes =
            {
                nameof(Patient.Naturality), nameof(Patient.ActivePatient), nameof(Patient.Phones), nameof(Patient.Addresses), nameof(Patient.FamilyMembers), nameof(Patient.Stays), nameof(Patient.FileAttachments),
                $"{nameof(Patient.PatientBenefits)}.{nameof(PatientBenefit.Benefit)}",
                $"{nameof(Patient.PatientExpenseTypes)}.{nameof(PatientExpenseType.ExpenseType)}",
                $"{nameof(Patient.PatientTreatmentTypes)}.{nameof(PatientTreatmentType.TreatmentType)}",
                $"{nameof(Patient.PatientAuxiliarAccessoryTypes)}.{nameof(PatientAuxiliarAccessoryType.AuxiliarAccessoryType)}",
                $"{nameof(Patient.PatientInformation)}.{nameof(PatientInformation.PatientInformationDoctors)}.{nameof(PatientInformationDoctor.Doctor)}",
                $"{nameof(Patient.PatientInformation)}.{nameof(PatientInformation.PatientInformationCancerTypes)}.{nameof(PatientInformationCancerType.CancerType)}",
                $"{nameof(Patient.PatientInformation)}.{nameof(PatientInformation.PatientInformationMedicines)}.{nameof(PatientInformationMedicine.Medicine)}",
                $"{nameof(Patient.PatientInformation)}.{nameof(PatientInformation.PatientInformationTreatmentPlaces)}.{nameof(PatientInformationTreatmentPlace.TreatmentPlace)}",
                $"{nameof(Patient.PatientInformation)}.{nameof(PatientInformation.PatientInformationServiceTypes)}.{nameof(PatientInformationServiceType.ServiceType)}",
            };

            Patient patient = await _patientService.FindByIdAsync(id, includes);

            if (patient == null)
            {
                return NotFound();
            }

            var patientPrintViewModel = new PatientPrintViewModel
            {
                FullName = $"{patient.FirstName} {patient.Surname}",
                RG = patient.RG,
                CPF = patient.CPF,
                CivilState = patient.CivilState,
                DateOfBirth = patient.DateOfBirth,
                FamiliarityGroup = patient.FamiliarityGroup,
                ForwardedToSupportHouse = patient.ForwardedToSupportHouse,
                JoinDate = patient.JoinDate,
                MonthlyIncome = patient.MonthlyIncome.ToString("N2"),
                Profession = patient.Profession,
                ImmediateNecessities = patient.ImmediateNecessities,
                ImmediateNecessitiesDateUpdated = patient.ImmediateNecessitiesDateUpdated,
                Sex = patient.Sex,
                CancerTypes = patient.PatientInformation.PatientInformationCancerTypes.Select(x => x.CancerType.Name).ToList(),
                Doctors = patient.PatientInformation.PatientInformationDoctors.Select(x => x.Doctor.Name).ToList(),
                Medicines = patient.PatientInformation.PatientInformationMedicines.Select(x => x.Medicine.Name).ToList(),
                TreatmentPlaces = patient.PatientInformation.PatientInformationTreatmentPlaces.Select(x => x.TreatmentPlace.City).ToList(),
                ServiceTypes = patient.PatientInformation.PatientInformationServiceTypes.Select(x => x.ServiceType.Name).ToList(),
                TreatmentBeginDate = patient.PatientInformation.TreatmentBeginDate,
                City = patient.Naturality.City,
                Country = patient.Naturality.Country,
                State = patient.Naturality.State,
                Phones = patient.Phones.Select(x => new PhoneViewModel
                {
                    Number = x.Number,
                    PhoneType = Enums.GetDisplayName(x.PhoneType),
                    ObservationNote = x.ObservationNote
                }),
                Addresses = patient.Addresses.Select(x => new AddressViewModel
                {
                    Street = x.Street,
                    Neighborhood = x.Neighborhood,
                    City = x.City,
                    HouseNumber = x.HouseNumber,
                    Complement = x.Complement,
                    ResidenceType = Enums.GetDisplayName(x.ResidenceType),
                    MonthlyAmmountResidence = x.MonthlyAmountResidence.ToString("N2"),
                    ObservationAddress = x.ObservationAddress,
                }),
                FamilyMembers = patient.FamilyMembers.Select(x => new FamilyMemberViewModel
                {
                    Name = x.Name,
                    Kinship = x.Kinship,
                    DateOfBirth = x.DateOfBirth.HasValue ? x.DateOfBirth.Value.ToShortDateString() : string.Empty,
                    Sex = Enums.GetDisplayName(x.Sex),
                    MonthlyIncome = x.MonthlyIncome.ToString("N2"),
                    Responsible = x.Responsible.ToString()
                }),
                PatientExpenseTypes = patient.PatientExpenseTypes.Select(x => new PatientExpenseTypeViewModel
                {
                    ExpenseType = x.ExpenseType.Name,
                    Frequency = Enums.GetDisplayName(x.ExpenseType.ExpenseTypeFrequency),
                    Value = x.Value.ToString("N2"),
                }),
                PatientTreatmentTypes = patient.PatientTreatmentTypes.Select(x => new PatientTreatmentTypeViewModel
                {
                    TreatmentType = x.TreatmentType.Name,
                    StartDate = x.StartDate.HasValue ? x.StartDate.Value.ToDateString() : string.Empty,
                    EndDate = x.EndDate.HasValue ? x.EndDate.Value.ToDateString() : string.Empty,
                    Note = x.Note,
                }),
                PatientAuxiliarAccessoryTypes = patient.PatientAuxiliarAccessoryTypes.Select(x => new PatientAuxiliarAccessoryTypeViewModel
                {
                    AuxiliarAccessoryType = x.AuxiliarAccessoryType.Name,
                    AuxiliarAccessoryTypeTime = Enums.GetDisplayName(x.AuxiliarAccessoryTypeTime),
                    DuoDate = x.DuoDate != DateTime.MinValue ? x.DuoDate.ToDateString() : "-",
                    Note = x.Note
                }),
                Benefits = patient.PatientBenefits.Select(x => new PatientBenefitViewModel
                {
                    Benefit = x.Benefit.Name,
                    Date = x.BenefitDate.ToShortDateString(),
                    Quantity = x.Quantity.ToString()
                }),
                Stays = patient.Stays.Select(x => new StayViewModel
                {
                    Date = x.StayDateTime.ToShortDateString(),
                    City = x.City,
                    Note = x.Note,
                }),
                Files = patient.FileAttachments.Select(x => new FileAttachmentViewModel
                {
                    Name = x.FileName,
                    Size = x.FileSize.ToString()
                }),
            };

            return View(patientPrintViewModel);
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
                var patient = new Patient
                {
                    FirstName = patientProfileForm.FirstName,
                    Surname = patientProfileForm.Surname,
                    RG = patientProfileForm.RG,
                    CPF = patientProfileForm.CPF,
                    FamiliarityGroup = patientProfileForm.FamiliarityGroup,
                    ForwardedToSupportHouse = patientProfileForm.ForwardedToSupportHouse,
                    Sex = patientProfileForm.Sex,
                    CivilState = patientProfileForm.CivilState,
                    DateOfBirth = DateTime.Parse(patientProfileForm.DateOfBirth),
                    JoinDate = DateTime.Parse(patientProfileForm.JoinDate),
                    Profession = patientProfileForm.Profession,
                    MonthlyIncome = double.TryParse(patientProfileForm.MonthlyIncome, out double monthlyIncome) ? monthlyIncome : 0,
                    ImmediateNecessities = patientProfileForm.ImmediateNecessities
                };

                if (!string.IsNullOrEmpty(patient.ImmediateNecessities))
                {
                    patient.ImmediateNecessitiesDateUpdated = DateTime.Now;
                }

                TaskResult result = await _patientService.CreateAsync(patient);

                if (result.Succeeded)
                {
                    return Ok(new { ok = true, url = Url.Action("AddPatientNaturality", new { id = patient.Naturality.NaturalityId }), title = "Adicionar Naturalidade" });
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
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
                Naturality naturality = await _naturalityService.FindByIdAsync(id, new[] { nameof(Naturality.Patient), $"{nameof(Naturality.Patient)}.{nameof(Patient.PatientInformation)}" });

                if (naturality == null)
                {
                    return NotFound();
                }

                naturality.City = naturalityForm.City;
                naturality.State = naturalityForm.State;
                naturality.Country = naturalityForm.Country;

                TaskResult result = await _naturalityService.UpdateAsync(naturality);

                if (result.Succeeded)
                {
                    Patient patient = await _patientService.FindByIdAsync(naturality.PatientId.ToString(), new[] { nameof(Patient.PatientInformation) });

                    return Ok(new
                    {
                        ok = true,
                        url = Url.Action("AddPatientInformation", new { id = patient.PatientInformation.PatientInformationId }),
                        title = "Adicionar Informação do Paciente"
                    });
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
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
                SelectTreatmentPlaces = await SelectHelper.GetTreatmentPlaceSelectAsync(_treatmentPlaceService),
                SelectServiceTypes = await SelectHelper.GetServiceTypesSelectAsync(_serviceTypeService),
            };

            return PartialView("Partials/_AddPatientInformation", patientInformationForm);
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientInformation(string id, PatientInformationFormModel patientInformationForm)
        {
            if (ModelState.IsValid)
            {
                string[] includes =
                {
                    nameof(PatientInformation.PatientInformationCancerTypes),
                    nameof(PatientInformation.PatientInformationDoctors),
                    nameof(PatientInformation.PatientInformationMedicines),
                    nameof(PatientInformation.PatientInformationTreatmentPlaces),
                    nameof(PatientInformation.PatientInformationServiceTypes)
                };

                PatientInformation patientInformation = await _patientInformationService.FindByIdAsync(id, includes);

                if (patientInformation == null)
                {
                    return NotFound();
                }

                patientInformation.TreatmentBeginDate = !string.IsNullOrEmpty(patientInformationForm.TreatmentBeginDate)
                    ? DateTime.Parse(patientInformationForm.TreatmentBeginDate)
                    : DateTime.MinValue;

                // Added Cancer Types to Patient Information
                await AddCancerTypes(patientInformationForm, patientInformation);

                // Added Doctor to Patient Information
                await AddDoctors(patientInformationForm, patientInformation);

                // Added Treatment Place to Patient Information
                await AddTreatmentPlaces(patientInformationForm, patientInformation);

                // Added Medicine to Patient Information
                await AddMedicines(patientInformationForm, patientInformation);

                // Added Service Types to Patient Information
                await AddServiceTypes(patientInformationForm, patientInformation);

                TaskResult result = await _patientInformationService.UpdateAsync(patientInformation);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            patientInformationForm.SelectDoctors = await SelectHelper.GetDoctorSelectAsync(_doctorService);
            patientInformationForm.SelectCancerTypes = await SelectHelper.GetCancerTypesSelectAsync(_cancerTypeService);
            patientInformationForm.SelectMedicines = await SelectHelper.GetMedicinesSelectAsync(_medicineService);
            patientInformationForm.SelectTreatmentPlaces = await SelectHelper.GetTreatmentPlaceSelectAsync(_treatmentPlaceService);

            return PartialView("Partials/_AddPatientInformation", patientInformationForm);
        }

        [HttpGet]
        public async Task<IActionResult> AddSocialObservation(string id)
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

            var socialObservationForm = new SocialObservationFormModel
            {
                Observations = patient.SocialObservation
            };

            return PartialView("Partials/_AddSocialObservation", socialObservationForm);
        }

        [Authorize(Roles = Roles.SocialAssistance)]
        [HttpPost]
        public async Task<IActionResult> AddSocialObservation(string id, SocialObservationFormModel socialObservationForm)
        {
            if (ModelState.IsValid)
            {
                Patient patient = await _patientService.FindByIdAsync(id);

                if (patient == null)
                {
                    return NotFound();
                }

                patient.SocialObservation = socialObservationForm.Observations;

                TaskResult result = await _patientService.UpdateAsync(patient);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddSocialObservation", socialObservationForm);
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
                ForwardedToSupportHouse = patient.ForwardedToSupportHouse,
                Sex = patient.Sex,
                CivilState = patient.CivilState,
                DateOfBirth = patient.DateOfBirth.ToDateString(),
                JoinDate = patient.JoinDate.ToDateString(),
                MonthlyIncome = patient.MonthlyIncome.ToString("N2"),
                Profession = patient.Profession,
                ImmediateNecessities = patient.ImmediateNecessities,
            };

            return PartialView("Partials/_EditPatientProfile", patientProfileForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientProfile(string id, PatientProfileFormModel patientProfileForm)
        {
            if (ModelState.IsValid)
            {
                Patient patient = await _patientService.FindByIdAsync(id);

                if (patient == null)
                {
                    return NotFound();
                }

                patient.FirstName = patientProfileForm.FirstName;
                patient.Surname = patientProfileForm.Surname;
                patient.RG = patientProfileForm.RG;
                patient.CPF = patientProfileForm.CPF;
                patient.FamiliarityGroup = patientProfileForm.FamiliarityGroup;
                patient.ForwardedToSupportHouse = patientProfileForm.ForwardedToSupportHouse;
                patient.Sex = patientProfileForm.Sex;
                patient.CivilState = patientProfileForm.CivilState;
                patient.DateOfBirth = DateTime.Parse(patientProfileForm.DateOfBirth);
                patient.JoinDate = DateTime.Parse(patientProfileForm.JoinDate);
                patient.Profession = patientProfileForm.Profession;
                patient.MonthlyIncome = double.TryParse(patientProfileForm.MonthlyIncome, out double monthlyIncome) ? monthlyIncome : 0;

                if (patient.ImmediateNecessities != patientProfileForm.ImmediateNecessities)
                {
                    patient.ImmediateNecessities = patientProfileForm.ImmediateNecessities;
                    patient.ImmediateNecessitiesDateUpdated = DateTime.Now;
                }

                TaskResult result = await _patientService.UpdateAsync(patient);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
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
                Naturality naturality = await _naturalityService.FindByIdAsync(id);

                if (naturality == null)
                {
                    return NotFound();
                }

                naturality.City = naturalityForm.City;
                naturality.State = naturalityForm.State;
                naturality.Country = naturalityForm.Country;

                TaskResult result = await _naturalityService.UpdateAsync(naturality);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
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

            string[] includes =
            {
                nameof(PatientInformation.PatientInformationCancerTypes),
                nameof(PatientInformation.PatientInformationDoctors),
                nameof(PatientInformation.PatientInformationMedicines),
                nameof(PatientInformation.PatientInformationTreatmentPlaces),
                nameof(PatientInformation.PatientInformationServiceTypes),
            };

            PatientInformation patientInformation = await _patientInformationService.FindByIdAsync(id, includes);

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
                SelectServiceTypes = await SelectHelper.GetServiceTypesSelectAsync(_serviceTypeService),
                CancerTypes = patientInformation.PatientInformationCancerTypes.Select(x => x.CancerTypeId.ToString()).ToList(),
                Doctors = patientInformation.PatientInformationDoctors.Select(x => x.DoctorId.ToString()).ToList(),
                Medicines = patientInformation.PatientInformationMedicines.Select(x => x.MedicineId.ToString()).ToList(),
                TreatmentPlaces = patientInformation.PatientInformationTreatmentPlaces.Select(x => x.TreatmentPlaceId.ToString()).ToList(),
                ServiceTypes = patientInformation.PatientInformationServiceTypes.Select(x => x.ServiceTypeId.ToString()).ToList(),
            };

            if (patientInformation.TreatmentBeginDate != DateTime.MinValue)
            {
                patientInformationForm.TreatmentBeginDate = patientInformation.TreatmentBeginDate.ToDateString();
            }

            return PartialView("Partials/_EditPatientInformation", patientInformationForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientInformation(string id, PatientInformationFormModel patientInformationForm)
        {
            if (ModelState.IsValid)
            {
                string[] includes =
                {
                    nameof(PatientInformation.PatientInformationCancerTypes),
                    nameof(PatientInformation.PatientInformationDoctors),
                    nameof(PatientInformation.PatientInformationMedicines),
                    nameof(PatientInformation.PatientInformationTreatmentPlaces),
                    nameof(PatientInformation.PatientInformationServiceTypes),
                };

                PatientInformation patientInformation = await _patientInformationService.FindByIdAsync(id, includes);

                if (patientInformation == null)
                {
                    return NotFound();
                }

                patientInformation.TreatmentBeginDate = !string.IsNullOrEmpty(patientInformationForm.TreatmentBeginDate)
                   ? DateTime.Parse(patientInformationForm.TreatmentBeginDate)
                   : DateTime.MinValue;

                // Added Cancer Types to Patient Information
                await UpdateCancerTypes(patientInformationForm, patientInformation);

                // Added Doctor to Patient Information
                await UpdateDoctors(patientInformationForm, patientInformation);

                // Added Treatment Places to Patient Information
                await UpdateTreatmentPlaces(patientInformationForm, patientInformation);

                // Added Medicine to Patient Information
                await UpdateMedicines(patientInformationForm, patientInformation);

                // Added Service Types to Patient Information
                await UpdateServiceTypes(patientInformationForm, patientInformation);

                TaskResult result = await _patientInformationService.UpdateAsync(patientInformation);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
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

            Patient patient = await _patientService.FindByIdAsync(id, new[] { nameof(Patient.ActivePatient), nameof(Patient.PatientInformation) });

            if (patient == null)
            {
                return NotFound();
            }

            patient.ActivePatient.Death = false;
            patient.ActivePatient.Discharge = false;
            patient.ActivePatient.ResidenceChange = false;
            var dateTime = DateTime.Parse(archivePatientForm.DateTime);
            switch (archivePatientForm.ArchivePatientType)
            {
                case Enums.ArchivePatientType.Death:
                    patient.ActivePatient.Death = true;
                    patient.ActivePatient.DeathDate = dateTime;
                    break;
                case Enums.ArchivePatientType.Discharge:
                    patient.ActivePatient.Discharge = true;
                    patient.ActivePatient.DischargeDate = dateTime;
                    break;
                case Enums.ArchivePatientType.ResidenceChange:
                    patient.ActivePatient.ResidenceChange = true;
                    patient.ActivePatient.ResidenceChangeDate = dateTime;
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
        public async Task<IActionResult> ActivePatient(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            Patient patient = await _patientService.FindByIdAsync(id, new[] { nameof(Patient.PatientInformation), nameof(Patient.ActivePatient) });

            if (patient == null)
            {
                return NotFound();
            }

            patient.ActivePatient.Discharge = false;
            patient.ActivePatient.ResidenceChange = false;
            TaskResult result = await _patientService.UpdateAsync(patient);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }

        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
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

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }

        #endregion

        #region Private Methods

        private async Task AddCancerTypes(PatientInformationFormModel patientInformationForm, PatientInformation patientInformation)
        {
            foreach (string cancerTypeValue in patientInformationForm.CancerTypes)
            {
                bool isCancerTypeInt = int.TryParse(cancerTypeValue, out int num);

                CancerType cancerType = null;
                if (isCancerTypeInt)
                {
                    cancerType = await _cancerTypeService.FindByIdAsync(cancerTypeValue);
                }

                cancerType ??= await ((CancerTypeRepository)_cancerTypeService).FindByNameAsync(cancerTypeValue) ?? new CancerType(cancerTypeValue);

                patientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType(cancerType));
            }
        }

        private async Task AddDoctors(PatientInformationFormModel patientInformationForm, PatientInformation patientInformation)
        {
            foreach (string doctorValue in patientInformationForm.Doctors)
            {
                Doctor doctor = null;
                bool isDoctorTypeInt = int.TryParse(doctorValue, out int num);
                if (isDoctorTypeInt)
                {
                    doctor = await _doctorService.FindByIdAsync(doctorValue);
                }

                doctor ??= await ((DoctorRepository)_doctorService).FindByNameAsync(doctorValue) ?? new Doctor(doctorValue);

                patientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor(doctor));
            }
        }

        private async Task AddTreatmentPlaces(PatientInformationFormModel patientInformationForm, PatientInformation patientInformation)
        {
            foreach (string treatmentPlaceValue in patientInformationForm.TreatmentPlaces)
            {
                TreatmentPlace treatmentPlace = int.TryParse(treatmentPlaceValue, out int num)
                    ? await _treatmentPlaceService.FindByIdAsync(treatmentPlaceValue)
                    : await ((TreatmentPlaceRepository)_treatmentPlaceService).FindByCityAsync(treatmentPlaceValue);

                if (treatmentPlace == null)
                {
                    treatmentPlace = new TreatmentPlace(treatmentPlaceValue);
                }

                patientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace(treatmentPlace));
            }
        }

        private async Task AddMedicines(PatientInformationFormModel patientInformationForm, PatientInformation patientInformation)
        {
            foreach (string item in patientInformationForm.Medicines)
            {
                Medicine medicine = int.TryParse(item, out int num)
                    ? await _medicineService.FindByIdAsync(item)
                    : await ((MedicineRepository)_medicineService).FindByNameAsync(item);

                if (medicine == null)
                {
                    medicine = new Medicine(item);
                }

                patientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine(medicine));
            }
        }

        private async Task AddServiceTypes(PatientInformationFormModel patientInformationForm, PatientInformation patientInformation)
        {
            foreach (string service in patientInformationForm.ServiceTypes)
            {
                bool isServiceInt = int.TryParse(service, out int num);

                ServiceType serviceType = null;
                if (isServiceInt)
                {
                    serviceType = await _serviceTypeService.FindByIdAsync(service);
                }

                serviceType ??= await ((ServiceTypeRepository)_serviceTypeService).FindByNameAsync(service) ?? new ServiceType(service);

                patientInformation.PatientInformationServiceTypes.Add(new PatientInformationServiceType(serviceType));
            }
        }

        private async Task UpdateCancerTypes(PatientInformationFormModel patientInformationForm, PatientInformation patientInformation)
        {
            if (patientInformationForm.CancerTypes.Count == 0)
            {
                patientInformation.PatientInformationCancerTypes.Clear();
            }
            else
            {
                // Filter to stay only selecteds that already stay on db
                patientInformation.PatientInformationCancerTypes = patientInformation.PatientInformationCancerTypes.Where(x => patientInformationForm.CancerTypes.Contains(x.CancerTypeId.ToString())).ToList();

                // Filter to stay on selected only that no stay on db yet
                patientInformationForm.CancerTypes = patientInformationForm.CancerTypes.Where(x => patientInformation.PatientInformationCancerTypes.All(y => y.CancerTypeId.ToString() != x)).ToList();

                // Add new
                foreach (string cancerTypeValue in patientInformationForm.CancerTypes)
                {
                    CancerType cancerType = null;
                    bool isCancerTypeInt = int.TryParse(cancerTypeValue, out int num);
                    if (isCancerTypeInt)
                    {
                        cancerType = await _cancerTypeService.FindByIdAsync(cancerTypeValue);
                    }

                    cancerType ??= await ((CancerTypeRepository)_cancerTypeService).FindByNameAsync(cancerTypeValue) ?? new CancerType(cancerTypeValue);

                    patientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType(cancerType));
                }
            }
        }

        private async Task UpdateDoctors(PatientInformationFormModel patientInformationForm, PatientInformation patientInformation)
        {
            if (patientInformationForm.Doctors.Count == 0)
            {
                patientInformation.PatientInformationDoctors.Clear();
            }
            else
            {
                // Filter to stay only selecteds that already stay on db
                patientInformation.PatientInformationDoctors = patientInformation.PatientInformationDoctors.Where(x => patientInformationForm.Doctors.Contains(x.DoctorId.ToString())).ToList();

                // Filter to stay on selected only that no stay on db yet
                patientInformationForm.Doctors = patientInformationForm.Doctors.Where(x => patientInformation.PatientInformationDoctors.All(y => y.DoctorId.ToString() != x)).ToList();

                // Add new
                foreach (string doctorValue in patientInformationForm.Doctors)
                {
                    Doctor doctor = null;
                    bool isDoctorTypeInt = int.TryParse(doctorValue, out int num);
                    if (isDoctorTypeInt)
                    {
                        doctor = await _doctorService.FindByIdAsync(doctorValue);
                    }

                    doctor ??= await ((DoctorRepository)_doctorService).FindByNameAsync(doctorValue) ?? new Doctor(doctorValue);

                    patientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor(doctor));
                }
            }
        }

        private async Task UpdateTreatmentPlaces(PatientInformationFormModel patientInformationForm, PatientInformation patientInformation)
        {
            if (patientInformationForm.TreatmentPlaces.Count == 0)
            {
                patientInformation.PatientInformationTreatmentPlaces.Clear();
            }
            else
            {
                // Filter to stay only selecteds that already stay on db
                patientInformation.PatientInformationTreatmentPlaces = patientInformation.PatientInformationTreatmentPlaces.Where(x => patientInformationForm.TreatmentPlaces.Contains(x.TreatmentPlaceId.ToString())).ToList();

                // Filter to stay on selected only that no stay on db yet
                patientInformationForm.TreatmentPlaces = patientInformationForm.TreatmentPlaces.Where(x => patientInformation.PatientInformationTreatmentPlaces.All(y => y.TreatmentPlaceId.ToString() != x)).ToList();

                // Add new
                foreach (string treatmentPlaceValue in patientInformationForm.TreatmentPlaces)
                {
                    TreatmentPlace treatmentPlace = null;
                    bool isTreatmentPlaceTypeInt = int.TryParse(treatmentPlaceValue, out int num);
                    if (isTreatmentPlaceTypeInt)
                    {
                        treatmentPlace = await _treatmentPlaceService.FindByIdAsync(treatmentPlaceValue);
                    }

                    treatmentPlace ??= await ((TreatmentPlaceRepository)_treatmentPlaceService).FindByCityAsync(treatmentPlaceValue) ?? new TreatmentPlace(treatmentPlaceValue);

                    patientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace(treatmentPlace));
                }
            }
        }

        private async Task UpdateMedicines(PatientInformationFormModel patientInformationForm, PatientInformation patientInformation)
        {
            if (patientInformationForm.Medicines.Count == 0)
            {
                patientInformation.PatientInformationMedicines.Clear();
            }
            else
            {
                // Filter to stay only selecteds that already stay on db
                patientInformation.PatientInformationMedicines = patientInformation.PatientInformationMedicines.Where(x => patientInformationForm.Medicines.Contains(x.MedicineId.ToString())).ToList();

                // Filter to stay on selected only that no stay on db yet
                patientInformationForm.Medicines = patientInformationForm.Medicines.Where(x => patientInformation.PatientInformationMedicines.All(y => y.MedicineId.ToString() != x)).ToList();

                // Add new
                foreach (string medicineValue in patientInformationForm.Medicines)
                {
                    Medicine medicine = null;
                    bool isMedicineTypeInt = int.TryParse(medicineValue, out int num);
                    if (isMedicineTypeInt)
                    {
                        medicine = await _medicineService.FindByIdAsync(medicineValue);
                    }

                    medicine ??= await ((MedicineRepository)_medicineService).FindByNameAsync(medicineValue) ?? new Medicine(medicineValue);

                    patientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine(medicine));
                }
            }
        }

        private async Task UpdateServiceTypes(PatientInformationFormModel patientInformationForm, PatientInformation patientInformation)
        {
            if (patientInformationForm.ServiceTypes.Count == 0)
            {
                patientInformation.PatientInformationServiceTypes.Clear();
            }
            else
            {
                // Filter to stay only selecteds that already stay on db
                patientInformation.PatientInformationServiceTypes = patientInformation.PatientInformationServiceTypes.Where(x => patientInformationForm.ServiceTypes.Contains(x.ServiceTypeId.ToString())).ToList();

                // Filter to stay on selected only that no stay on db yet
                patientInformationForm.ServiceTypes = patientInformationForm.ServiceTypes.Where(x => patientInformation.PatientInformationServiceTypes.All(y => y.ServiceTypeId.ToString() != x)).ToList();

                // Add new
                foreach (string serviceValue in patientInformationForm.ServiceTypes)
                {
                    ServiceType serviceType = null;
                    bool isServiceTypeInt = int.TryParse(serviceValue, out int num);
                    if (isServiceTypeInt)
                    {
                        serviceType = await _serviceTypeService.FindByIdAsync(serviceValue);
                    }

                    serviceType ??= await ((ServiceTypeRepository)_serviceTypeService).FindByNameAsync(serviceValue) ?? new ServiceType(serviceValue);

                    patientInformation.PatientInformationServiceTypes.Add(new PatientInformationServiceType(serviceType));
                }
            }
        }

        #endregion
    }
}