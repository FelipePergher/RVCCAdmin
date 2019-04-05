using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormModel;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using LigaCancer.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using LigaCancer.Data.Models.RelationModels;
using LigaCancer.Code.Interface;
using LigaCancer.Models.SearchModel;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PatientController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<Patient> _patientService;
        private readonly IDataStore<Doctor> _doctorService;
        private readonly IDataStore<TreatmentPlace> _treatmentPlaceService;
        private readonly IDataStore<CancerType> _cancerTypeService;
        private readonly IDataStore<Medicine> _medicineService;

        public PatientController(
            IDataStore<Patient> patientService,
            IDataStore<Doctor> doctorService,
            IDataStore<TreatmentPlace> treatmentPlaceService,
            IDataStore<CancerType> cancerTypeService,
            IDataStore<Medicine> medicineService,
            UserManager<ApplicationUser> userManager)
        {
            _patientService = patientService;
            _userManager = userManager;
            _doctorService = doctorService;
            _cancerTypeService = cancerTypeService;
            _treatmentPlaceService = treatmentPlaceService;
            _medicineService = medicineService;
        }

        public IActionResult Index()
        {
            PatientSearchModel patientSearch = new PatientSearchModel {
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

        public IActionResult AddPatient()
        {
            PatientFormModel patientForm = new PatientFormModel
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
                }).ToList(),
                DateOfBirth = DateTime.Now
            };

            return PartialView("_AddPatient", patientForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPatient(PatientFormModel patientForm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                Patient patient = new Patient
                {
                    FirstName = patientForm.FirstName,
                    Surname = patientForm.Surname,
                    RG = patientForm.RG,
                    CPF = patientForm.CPF,
                    FamiliarityGroup = patientForm.FamiliarityGroup,
                    Sex = patientForm.Sex,
                    CivilState = patientForm.CivilState,
                    DateOfBirth = patientForm.DateOfBirth,
                    Profession = patientForm.Profession,
                    Naturality = new Naturality
                    {
                        City = patientForm.Naturality.City,
                        State = patientForm.Naturality.State,
                        Country = patientForm.Naturality.Country,
                        UserCreated = user,
                    },
                    UserCreated = user,
                    Family = new Family
                    {
                        MonthlyIncome = patientForm.MonthlyIncome != null ? patientForm.MonthlyIncome : 0,
                        FamilyIncome = patientForm.MonthlyIncome != null ? (double)patientForm.MonthlyIncome : 0,
                        PerCapitaIncome = patientForm.MonthlyIncome != null ? (double)patientForm.MonthlyIncome : 0
                    }
                };

                //Added Cancer Types to Patient Information
                foreach (string item in patientForm.PatientInformation.CancerTypes)
                {
                    CancerType cancerType = int.TryParse(item, out int num) ? await _cancerTypeService.FindByIdAsync(item) : await ((CancerTypeStore)_cancerTypeService).FindByNameAsync(item);
                    if (cancerType == null)
                    {
                        cancerType = new CancerType
                        {
                            Name = item,
                            UserCreated = user
                        };
                        TaskResult taskResult = await _cancerTypeService.CreateAsync(cancerType);
                        if (taskResult.Succeeded)
                        {
                            patient.PatientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType
                            {
                                CancerType = cancerType
                            });
                        }
                    }
                    else
                    {
                        patient.PatientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType
                        {
                            CancerType = cancerType
                        });
                    }
                    
                }

                //Added Doctor to Patient Information
                foreach (string item in patientForm.PatientInformation.Doctors)
                {
                    Doctor doctor = int.TryParse(item, out int num) ? await _doctorService.FindByIdAsync(item) : await ((DoctorStore)_doctorService).FindByNameAsync(item);
                    if (doctor == null)
                    {
                        doctor = new Doctor
                        {
                            Name = item,
                            UserCreated = user
                        };
                        TaskResult taskResult = await _doctorService.CreateAsync(doctor);
                        if (taskResult.Succeeded)
                        {
                            patient.PatientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor
                            {
                                Doctor = doctor
                            });
                        }
                    }
                    else
                    {
                        patient.PatientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor
                        {
                            Doctor = doctor
                        });
                    }
                }

                //Added Treatment Place to Patient Information
                foreach (string item in patientForm.PatientInformation.TreatmentPlaces)
                {
                    TreatmentPlace treatmentPlace = int.TryParse(item, out int num) ? 
                        await _treatmentPlaceService.FindByIdAsync(item) : await ((TreatmentPlaceStore)_treatmentPlaceService).FindByCityAsync(item);
                    if (treatmentPlace == null)
                    {
                        treatmentPlace = new TreatmentPlace
                        {
                            City = item,
                            UserCreated = user
                        };
                        TaskResult taskResult = await _treatmentPlaceService.CreateAsync(treatmentPlace);
                        if (taskResult.Succeeded)
                        {
                            patient.PatientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace
                            {
                                TreatmentPlace = treatmentPlace
                            });
                        }
                    }
                    else
                    {
                        patient.PatientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace
                        {
                            TreatmentPlace = treatmentPlace
                        });
                    }
                    
                }

                //Added Medicine to Patient Information
                foreach (string item in patientForm.PatientInformation.Medicines)
                {
                    Medicine medicine = int.TryParse(item, out int num) ?
                        await _medicineService.FindByIdAsync(item) : await ((MedicineStore)_medicineService).FindByNameAsync(item);

                    if (medicine == null)
                    {
                        medicine = new Medicine
                        {
                            Name = item,
                            UserCreated = user
                        };
                        TaskResult taskResult = await _medicineService.CreateAsync(medicine);
                        if (taskResult.Succeeded)
                        {
                            patient.PatientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine
                            {
                                Medicine = medicine
                            });
                        }
                    }
                    else
                    {
                        patient.PatientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine
                        {
                            Medicine = medicine
                        });
                    }
                    
                }

                TaskResult result = await _patientService.CreateAsync(patient);
                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

            patientForm.SelectDoctors = _doctorService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.DoctorId.ToString()
            }).ToList();

            patientForm.SelectCancerTypes = _cancerTypeService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CancerTypeId.ToString()
            }).ToList();

            patientForm.SelectMedicines = _medicineService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.MedicineId.ToString()
            }).ToList();

            patientForm.SelectTreatmentPlaces = _treatmentPlaceService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.TreatmentPlaceId.ToString()
            }).ToList();

            return PartialView("_AddPatient", patientForm);
        }

        public async Task<IActionResult> EditPatient(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            
            BaseSpecification<Patient> specification = new BaseSpecification<Patient>(
                x => x.Naturality, x => x.PatientInformation, x => x.Family,
                x => x.PatientInformation.PatientInformationCancerTypes,
                x => x.PatientInformation.PatientInformationDoctors,
                x => x.PatientInformation.PatientInformationMedicines,
                x => x.PatientInformation.PatientInformationTreatmentPlaces
            );
            Patient patient = await _patientService.FindByIdAsync(id, specification);
            
            if (patient == null) return BadRequest();

            PatientFormModel patientForm = new PatientFormModel
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
                }).ToList(),
                CivilState = patient.CivilState,
                CPF = patient.CPF,
                DateOfBirth = patient.DateOfBirth,
                FamiliarityGroup = patient.FamiliarityGroup,
                FirstName = patient.FirstName,
                Naturality = new NaturalityFormModel
                {
                    City = patient.Naturality.City,
                    Country = patient.Naturality.Country,
                    State = patient.Naturality.State
                },
                PatientId = patient.PatientId,
                PatientInformation = new PatientInformationFormModel
                {
                    CancerTypes = patient.PatientInformation.PatientInformationCancerTypes.Select(x => x.CancerType.CancerTypeId.ToString()).ToList(),
                    Doctors = patient.PatientInformation.PatientInformationDoctors.Select(x => x.Doctor.DoctorId.ToString()).ToList(),
                    Medicines = patient.PatientInformation.PatientInformationMedicines.Select(x => x.Medicine.MedicineId.ToString()).ToList(),
                    TreatmentPlaces = patient.PatientInformation.PatientInformationTreatmentPlaces.Select(x => x.TreatmentPlace.TreatmentPlaceId.ToString()).ToList()
                },
                Profession = patient.Profession,
                RG = patient.RG,
                Sex = patient.Sex,
                Surname = patient.Surname,
                MonthlyIncome = patient.Family.MonthlyIncome
            };

            return PartialView("_EditPatient", patientForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPatient(string id, PatientFormModel patientForm)
        {
            if (ModelState.IsValid)
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(
                    x => x.Naturality, x => x.PatientInformation, x => x.Family, x => x.Family.FamilyMembers,
                    x => x.PatientInformation.PatientInformationCancerTypes, 
                    x => x.PatientInformation.PatientInformationDoctors,
                    x => x.PatientInformation.PatientInformationMedicines,
                    x => x.PatientInformation.PatientInformationTreatmentPlaces
                );
                specification.IncludeStrings.Add("PatientInformation.PatientInformationDoctors.Doctor");
                specification.IncludeStrings.Add("PatientInformation.PatientInformationMedicines.Medicine");
                specification.IncludeStrings.Add("PatientInformation.PatientInformationCancerTypes.CancerType");
                specification.IncludeStrings.Add("PatientInformation.PatientInformationTreatmentPlaces.TreatmentPlace");

                Patient patient = await _patientService.FindByIdAsync(id, specification);
                ApplicationUser user = await _userManager.GetUserAsync(User);


                //Added Cancer Types to Patient Information
                if(patientForm.PatientInformation.CancerTypes.Count == 0)
                {
                    patient.PatientInformation.PatientInformationCancerTypes.Clear();
                }
                else
                {
                    //Remove not selected of database
                    List<PatientInformationCancerType> removePatientInformationCancerTypes = new List<PatientInformationCancerType>();
                    foreach (PatientInformationCancerType item in patient.PatientInformation.PatientInformationCancerTypes)
                    {
                        if (patientForm.PatientInformation.CancerTypes.FirstOrDefault(x => x == item.CancerType.Name) == null)
                        {
                            removePatientInformationCancerTypes.Add(item);
                        }
                    }
                    patient.PatientInformation.PatientInformationCancerTypes = patient.PatientInformation.PatientInformationCancerTypes.Except(removePatientInformationCancerTypes).ToList();

                    //Add news cancer types
                    foreach (string item in patientForm.PatientInformation.CancerTypes)
                    {
                        CancerType cancerType = int.TryParse(item, out int num) ? await _cancerTypeService.FindByIdAsync(item) : await ((CancerTypeStore)_cancerTypeService).FindByNameAsync(item);
                        if (cancerType == null)
                        {
                            cancerType = new CancerType
                            {
                                Name = item,
                                UserCreated = user
                            };
                            TaskResult taskResult = await _cancerTypeService.CreateAsync(cancerType);
                            if (taskResult.Succeeded)
                            {
                                patient.PatientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType
                                {
                                    CancerType = cancerType
                                });
                            }
                        }
                        else
                        {
                            if(patient.PatientInformation.PatientInformationCancerTypes.FirstOrDefault(x => x.CancerTypeId == cancerType.CancerTypeId) == null)
                            {
                                patient.PatientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType
                                {
                                    CancerType = cancerType
                                });
                            }
                        }

                    }
                }

                //Added Doctor to Patient Information
                if(patientForm.PatientInformation.Doctors.Count == 0)
                {
                    patient.PatientInformation.PatientInformationDoctors.Clear();
                }
                else
                {
                    //Remove not selected of database
                    List<PatientInformationDoctor> removePatientInformationDoctors = new List<PatientInformationDoctor>();
                    foreach (PatientInformationDoctor item in patient.PatientInformation.PatientInformationDoctors)
                    {
                        if (patientForm.PatientInformation.Doctors.FirstOrDefault(x => x == item.Doctor.Name) == null)
                        {
                            removePatientInformationDoctors.Add(item);
                        }
                    }
                    patient.PatientInformation.PatientInformationDoctors = patient.PatientInformation.PatientInformationDoctors.Except(removePatientInformationDoctors).ToList();

                    //Add news doctors
                    foreach (string item in patientForm.PatientInformation.Doctors)
                    {
                        Doctor doctor = int.TryParse(item, out int num) ? await _doctorService.FindByIdAsync(item) : await ((DoctorStore)_doctorService).FindByNameAsync(item);
                        if (doctor == null)
                        {
                            doctor = new Doctor
                            {
                                Name = item,
                                UserCreated = user
                            };
                            TaskResult taskResult = await _doctorService.CreateAsync(doctor);
                            if (taskResult.Succeeded)
                            {
                                patient.PatientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor
                                {
                                    Doctor = doctor
                                });
                            }
                        }
                        else
                        {
                            if(patient.PatientInformation.PatientInformationDoctors.FirstOrDefault(x => x.DoctorId == doctor.DoctorId) == null)
                            {
                                patient.PatientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor
                                {
                                    Doctor = doctor
                                });
                            }
                        }
                    }
                   
                }

                //Added Treatment Place to Patient Information
                if (patientForm.PatientInformation.TreatmentPlaces.Count == 0)
                {
                    patient.PatientInformation.PatientInformationTreatmentPlaces.Clear();
                }
                else
                {
                    //Remove not selected of database
                    List<PatientInformationTreatmentPlace> removePatientInformationTreatmentPlaces = new List<PatientInformationTreatmentPlace>();
                    foreach (PatientInformationTreatmentPlace item in patient.PatientInformation.PatientInformationTreatmentPlaces)
                    {
                        if (patientForm.PatientInformation.TreatmentPlaces.FirstOrDefault(x => x == item.TreatmentPlace.City) == null)
                        {
                            removePatientInformationTreatmentPlaces.Add(item);
                        }
                    }
                    patient.PatientInformation.PatientInformationTreatmentPlaces = patient.PatientInformation.PatientInformationTreatmentPlaces.Except(removePatientInformationTreatmentPlaces).ToList();

                    //Add new Treatment Place
                    foreach (string item in patientForm.PatientInformation.TreatmentPlaces)
                    {
                        TreatmentPlace treatmentPlace = int.TryParse(item, out int num) ?
                            await _treatmentPlaceService.FindByIdAsync(item) : await ((TreatmentPlaceStore)_treatmentPlaceService).FindByCityAsync(item);
                        if (treatmentPlace == null)
                        {
                            treatmentPlace = new TreatmentPlace
                            {
                                City = item,
                                UserCreated = user
                            };
                            TaskResult taskResult = await _treatmentPlaceService.CreateAsync(treatmentPlace);
                            if (taskResult.Succeeded)
                            {
                                patient.PatientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace
                                {
                                    TreatmentPlace = treatmentPlace
                                });
                            }
                        }
                        else
                        {
                            if (patient.PatientInformation.PatientInformationTreatmentPlaces.FirstOrDefault(x => x.TreatmentPlaceId == treatmentPlace.TreatmentPlaceId) == null)
                            {
                                patient.PatientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace
                                {
                                    TreatmentPlace = treatmentPlace
                                });
                            }
                        }
                    }
                }

                //Added Treatment Place to Patient Information
                if (patientForm.PatientInformation.Medicines.Count == 0)
                {
                    patient.PatientInformation.PatientInformationMedicines.Clear();
                }
                else
                {

                    //Remove not selected of database
                    List<PatientInformationMedicine> removePatientInformationMedicines = new List<PatientInformationMedicine>();
                    foreach (PatientInformationMedicine item in patient.PatientInformation.PatientInformationMedicines)
                    {
                        if (patientForm.PatientInformation.TreatmentPlaces.FirstOrDefault(x => x == item.Medicine.Name) == null)
                        {
                            removePatientInformationMedicines.Add(item);
                        }
                    }
                    patient.PatientInformation.PatientInformationMedicines = patient.PatientInformation.PatientInformationMedicines.Except(removePatientInformationMedicines).ToList();

                    //Added Medicine to Patient Information
                    foreach (var item in patientForm.PatientInformation.Medicines)
                    {
                        Medicine medicine = int.TryParse(item, out int num) ?
                            await _medicineService.FindByIdAsync(item) : await ((MedicineStore)_medicineService).FindByNameAsync(item);

                        if (medicine == null)
                        {
                            medicine = new Medicine
                            {
                                Name = item,
                                UserCreated = user
                            };
                            TaskResult taskResult = await _medicineService.CreateAsync(medicine);
                            if (taskResult.Succeeded)
                            {
                                patient.PatientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine
                                {
                                    Medicine = medicine
                                });
                            }
                        }
                        else
                        {
                            if (patient.PatientInformation.PatientInformationMedicines.FirstOrDefault(x => x.MedicineId == medicine.MedicineId) == null)
                            {
                                patient.PatientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine
                                {
                                    Medicine = medicine
                                });
                            }
                        }
                    }
                }

                patient.UpdatedDate = DateTime.Now;
                patient.UserUpdated = user;

                patient.FirstName = patientForm.FirstName;
                patient.Surname = patientForm.Surname;
                patient.RG = patientForm.RG;
                patient.CPF = patientForm.CPF;
                patient.FamiliarityGroup = patientForm.FamiliarityGroup;
                patient.Sex = patientForm.Sex;
                patient.CivilState = patientForm.CivilState;
                patient.DateOfBirth = patientForm.DateOfBirth;
                patient.Naturality.City = patientForm.Naturality.City;
                patient.Naturality.State = patientForm.Naturality.State;
                patient.Naturality.Country = patientForm.Naturality.Country;
                patient.Profession = patientForm.Profession;

                patient.Family.FamilyIncome -= (double)patient.Family.MonthlyIncome;
                patient.Family.FamilyIncome += (double)patientForm.MonthlyIncome;
                patient.Family.MonthlyIncome = patientForm.MonthlyIncome;

                patient.Family.PerCapitaIncome = patient.Family.FamilyIncome / (patient.Family.FamilyMembers.Count + 1);

                TaskResult result = await _patientService.UpdateAsync(patient);
                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

            patientForm.SelectDoctors = _doctorService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.DoctorId.ToString()
            }).ToList();

            patientForm.SelectCancerTypes = _cancerTypeService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CancerTypeId.ToString()
            }).ToList();

            patientForm.SelectMedicines = _medicineService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.MedicineId.ToString()
            }).ToList();

            patientForm.SelectTreatmentPlaces = _treatmentPlaceService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.TreatmentPlaceId.ToString()
            }).ToList();

            return PartialView("_EditPatient", patientForm);
        }

        public IActionResult DisablePatient(string id)
        {
            DisablePatientFormModel disablePatientForm = new DisablePatientFormModel {
                DateTime = DateTime.Now
            };
            return PartialView("_DisablePatient", disablePatientForm);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DisablePatient(string id, DisablePatientFormModel disablePatientForm)
        {
            if (!string.IsNullOrEmpty(id))
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.PatientInformation, x => x.PatientInformation.ActivePatient);
                Patient patient = await _patientService.FindByIdAsync(id, specification);
                if (patient == null) return RedirectToAction("Index");

                switch (disablePatientForm.DisablePatientType)
                {
                    case Globals.DisablePatientType.death:
                        patient.PatientInformation.ActivePatient.Death = true;
                        patient.PatientInformation.ActivePatient.DeathDate = disablePatientForm.DateTime;
                        break;
                    case Globals.DisablePatientType.discharge:
                        patient.PatientInformation.ActivePatient.Discharge = true;
                        patient.PatientInformation.ActivePatient.DischargeDate = disablePatientForm.DateTime;
                        break;
                }

                TaskResult result = await _patientService.DeleteAsync(patient);

                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
                return PartialView("_DeletePatient", patient.FirstName);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DetailsPatient(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            BaseSpecification<Patient> specification = new BaseSpecification<Patient>(
                   x => x.Naturality, x => x.Family, x => x.FileAttachments, x => x.PatientInformation,
                   x => x.PatientInformation.PatientInformationCancerTypes,
                   x => x.PatientInformation.PatientInformationDoctors,
                   x => x.PatientInformation.PatientInformationMedicines,
                   x => x.PatientInformation.PatientInformationTreatmentPlaces
               );
            specification.IncludeStrings.Add("PatientInformation.PatientInformationDoctors.Doctor");
            specification.IncludeStrings.Add("PatientInformation.PatientInformationMedicines.Medicine");
            specification.IncludeStrings.Add("PatientInformation.PatientInformationCancerTypes.CancerType");
            specification.IncludeStrings.Add("PatientInformation.PatientInformationTreatmentPlaces.TreatmentPlace");

            Patient patient = await _patientService.FindByIdAsync(id, specification);

            if (patient == null)
            {
                return NotFound();
            }
            
            PatientShowFormModel patientShowForm = new PatientShowFormModel
            {
                PatientId = patient.PatientId,
                Name = patient.FirstName + " " + patient.Surname,
                CivilState = patient.CivilState,
                CPF = patient.CPF,
                DateOfBirth = patient.DateOfBirth,
                FamiliarityGroup = patient.FamiliarityGroup,
                Profession = patient.Profession,
                RG = patient.RG,
                Sex = patient.Sex,
                Family = patient.Family,
                Naturality = new NaturalityFormModel
                {
                    City = patient.Naturality.City,
                    Country = patient.Naturality.Country,
                    State = patient.Naturality.State
                },
                FileAttachments = patient.FileAttachments,
                PatientInformation = new PatientInformationFormModel
                {
                    CancerTypes = patient.PatientInformation.PatientInformationCancerTypes.Select(x => x.CancerType.Name).ToList(),
                    Doctors = patient.PatientInformation.PatientInformationDoctors.Select(x => x.Doctor.Name).ToList(),
                    Medicines = patient.PatientInformation.PatientInformationMedicines.Select(x => x.Medicine.Name).ToList(),
                    TreatmentPlaces = patient.PatientInformation.PatientInformationTreatmentPlaces.Select(x => x.TreatmentPlace.City).ToList(),
                }
            };

            return View(patientShowForm);
        }

        //[HttpGet]
        //public async Task<IActionResult> ActivePatient(string id)
        //{
        //    Patient patient = await _patientService.FindByIdAsync(id);
        //    return PartialView("_ActivePatient", $"{patient.FirstName} {patient.Surname}");
        //}

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> ActivePatient(string id, IFormCollection form)
        //{
        //    if (string.IsNullOrEmpty(id)) return BadRequest();

        //    BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.PatientInformation, x => x.PatientInformation.ActivePatient);
        //    Patient patient = await _patientService.FindByIdAsync(id, specification);
        //    if (patient == null) return RedirectToAction("Index");

        //    patient.PatientInformation.ActivePatient.Discharge = false;
        //    TaskResult result = ((PatientStore)_patientService).ActivePatient(patient);
        //    if (result.Succeeded)
        //    {
        //        return StatusCode(200, "200");
        //    }
        //    ModelState.AddErrors(result);
        //    return PartialView("_ActivePatient", $"{patient.FirstName} {patient.Surname}");
        //}

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
    }
}