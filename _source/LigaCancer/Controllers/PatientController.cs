using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.MedicalViewModels;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using LigaCancer.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using LigaCancer.Data.Models.ManyToManyModels;
using LigaCancer.Code.Interface;
using LigaCancer.Models.SearchViewModels;
using Microsoft.AspNetCore.Http;

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
        private readonly IDataStore<Profession> _professionService;

        public PatientController(
            IDataStore<Patient> patientService,
            IDataStore<Doctor> doctorService,
            IDataStore<TreatmentPlace> treatmentPlaceService,
            IDataStore<CancerType> cancerTypeService,
            IDataStore<Medicine> medicineService,
            IDataStore<Profession> professionService,
            UserManager<ApplicationUser> userManager)
        {
            _patientService = patientService;
            _userManager = userManager;
            _doctorService = doctorService;
            _cancerTypeService = cancerTypeService;
            _treatmentPlaceService = treatmentPlaceService;
            _professionService = professionService;
            _medicineService = medicineService;
        }

        public IActionResult Index()
        {
            PatientSearchViewModel patientSearchViewModel = new PatientSearchViewModel {
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
            return View(patientSearchViewModel);
        }

        public IActionResult AddPatient()
        {
            PatientViewModel patientViewModel = new PatientViewModel
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

            patientViewModel.SelectProfessions.AddRange(_professionService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ProfessionId.ToString()
            }).ToList());

            return PartialView("_AddPatient", patientViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddPatient(PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(this.User);

                Patient patient = new Patient
                {
                    FirstName = model.FirstName,
                    Surname = model.Surname,
                    RG = model.RG,
                    CPF = model.CPF,
                    FamiliarityGroup = model.FamiliarityGroup,
                    Sex = model.Sex,
                    CivilState = model.CivilState,
                    DateOfBirth = model.DateOfBirth,
                    Naturality = new Naturality
                    {
                        City = model.Naturality.City,
                        State = model.Naturality.State,
                        Country = model.Naturality.Country,
                        UserCreated = user,
                    },
                    UserCreated = user
                };

                Profession profession = int.TryParse(model.Profession, out int num) ? 
                    await _professionService.FindByIdAsync(model.Profession) : await ((ProfessionStore)_professionService).FindByNameAsync(model.Profession);
                if (profession == null && model.Profession != null)
                {
                    profession = new Profession
                    {
                        Name = model.Profession,
                        UserCreated = user
                    };
                    TaskResult taskResult = await _professionService.CreateAsync(profession);
                    if (taskResult.Succeeded)
                    {
                        patient.Profession = profession;
                    }
                }
                else if(profession != null)
                {
                    patient.Profession = profession;
                }

                //Added Cancer Types to Patient Information
                foreach (var item in model.PatientInformation.CancerTypes)
                {
                    CancerType cancerType = int.TryParse(item, out num) ? await _cancerTypeService.FindByIdAsync(item) : await ((CancerTypeStore)_cancerTypeService).FindByNameAsync(item);
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
                foreach (var item in model.PatientInformation.Doctors)
                {
                    Doctor doctor = int.TryParse(item, out num) ? await _doctorService.FindByIdAsync(item) : await ((DoctorStore)_doctorService).FindByNameAsync(item);
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
                foreach (var item in model.PatientInformation.TreatmentPlaces)
                {
                    TreatmentPlace treatmentPlace = int.TryParse(item, out num) ? 
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
                foreach (var item in model.PatientInformation.Medicines)
                {
                    Medicine medicine = int.TryParse(item, out num) ?
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

            model.SelectProfessions = _professionService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ProfessionId.ToString()
            }).ToList();

            model.SelectDoctors = _doctorService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.DoctorId.ToString()
            }).ToList();

            model.SelectCancerTypes = _cancerTypeService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CancerTypeId.ToString()
            }).ToList();

            model.SelectMedicines = _medicineService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.MedicineId.ToString()
            }).ToList();

            model.SelectTreatmentPlaces = _treatmentPlaceService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.TreatmentPlaceId.ToString()
            }).ToList();

            return PartialView("_AddPatient", model);
        }


        public async Task<IActionResult> EditPatient(string id)
        {
            PatientViewModel patientViewModel = new PatientViewModel
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
            };

            patientViewModel.SelectProfessions.AddRange(_professionService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ProfessionId.ToString()
            }).ToList());

            if (!string.IsNullOrEmpty(id))
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(
                    x => x.Naturality, x => x.Profession, x => x.PatientInformation,
                    x => x.PatientInformation.PatientInformationCancerTypes,
                    x => x.PatientInformation.PatientInformationDoctors,
                    x => x.PatientInformation.PatientInformationMedicines,
                    x => x.PatientInformation.PatientInformationTreatmentPlaces
                );
                Patient patient = await _patientService.FindByIdAsync(id, specification);
                if (patient != null)
                {
                    patientViewModel.CivilState = patient.CivilState;
                    patientViewModel.CPF = patient.CPF;
                    patientViewModel.DateOfBirth = patient.DateOfBirth;
                    patientViewModel.FamiliarityGroup = patient.FamiliarityGroup;
                    patientViewModel.FirstName = patient.FirstName;
                    patientViewModel.Naturality = new NaturalityViewModel
                    {
                        City = patient.Naturality.City,
                        Country = patient.Naturality.Country,
                        State = patient.Naturality.State
                    };
                    patientViewModel.PatientId = patient.PatientId;
                    patientViewModel.PatientInformation = new PatientInformationViewModel
                    {
                        CancerTypes = patient.PatientInformation.PatientInformationCancerTypes.Select(x => x.CancerType.CancerTypeId.ToString()).ToList(),
                        Doctors = patient.PatientInformation.PatientInformationDoctors.Select(x => x.Doctor.DoctorId.ToString()).ToList(),
                        Medicines = patient.PatientInformation.PatientInformationMedicines.Select(x => x.Medicine.MedicineId.ToString()).ToList(),
                        TreatmentPlaces = patient.PatientInformation.PatientInformationTreatmentPlaces.Select(x => x.TreatmentPlace.TreatmentPlaceId.ToString()).ToList()
                    };
                    patientViewModel.Profession = patient.Profession?.ProfessionId.ToString();
                    patientViewModel.RG = patient.RG;
                    patientViewModel.Sex = patient.Sex;
                    patientViewModel.Surname = patient.Surname;
                }
            }

            return PartialView("_EditPatient", patientViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPatient(string id, PatientViewModel model)
        {
            if (ModelState.IsValid)
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(
                    x => x.Naturality, x => x.Profession, x => x.PatientInformation,
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
                ApplicationUser user = await _userManager.GetUserAsync(this.User);

                Profession profession = int.TryParse(model.Profession, out int num) ?
                    await _professionService.FindByIdAsync(model.Profession) : await ((ProfessionStore)_professionService).FindByNameAsync(model.Profession);
                if (profession == null && model.Profession != null)
                {
                    profession = new Profession
                    {
                        Name = model.Profession,
                        UserCreated = user
                    };
                    TaskResult taskResult = await _professionService.CreateAsync(profession);
                    if (taskResult.Succeeded)
                    {
                        patient.Profession = profession;
                    }
                }
                else if(profession != null)
                {
                    patient.Profession = profession;
                }

                //Added Cancer Types to Patient Information
                if(model.PatientInformation.CancerTypes.Count == 0)
                {
                    patient.PatientInformation.PatientInformationCancerTypes.Clear();
                }
                else
                {
                    //Remove not selected of database
                    List<PatientInformationCancerType> removePatientInformationCancerTypes = new List<PatientInformationCancerType>();
                    foreach (var item in patient.PatientInformation.PatientInformationCancerTypes)
                    {
                        if (model.PatientInformation.CancerTypes.FirstOrDefault(x => x == item.CancerType.Name) == null)
                        {
                            removePatientInformationCancerTypes.Add(item);
                        }
                    }
                    patient.PatientInformation.PatientInformationCancerTypes = patient.PatientInformation.PatientInformationCancerTypes.Except(removePatientInformationCancerTypes).ToList();

                    //Add news cancer types
                    foreach (var item in model.PatientInformation.CancerTypes)
                    {
                        CancerType cancerType = int.TryParse(item, out num) ? await _cancerTypeService.FindByIdAsync(item) : await ((CancerTypeStore)_cancerTypeService).FindByNameAsync(item);
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
                if(model.PatientInformation.Doctors.Count == 0)
                {
                    patient.PatientInformation.PatientInformationDoctors.Clear();
                }
                else
                {
                    //Remove not selected of database
                    List<PatientInformationDoctor> removePatientInformationDoctors = new List<PatientInformationDoctor>();
                    foreach (var item in patient.PatientInformation.PatientInformationDoctors)
                    {
                        if (model.PatientInformation.Doctors.FirstOrDefault(x => x == item.Doctor.Name) == null)
                        {
                            removePatientInformationDoctors.Add(item);
                        }
                    }
                    patient.PatientInformation.PatientInformationDoctors = patient.PatientInformation.PatientInformationDoctors.Except(removePatientInformationDoctors).ToList();

                    //Add news doctors
                    foreach (var item in model.PatientInformation.Doctors)
                    {
                        Doctor doctor = int.TryParse(item, out num) ? await _doctorService.FindByIdAsync(item) : await ((DoctorStore)_doctorService).FindByNameAsync(item);
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
                if (model.PatientInformation.TreatmentPlaces.Count == 0)
                {
                    patient.PatientInformation.PatientInformationTreatmentPlaces.Clear();
                }
                else
                {
                    //Remove not selected of database
                    List<PatientInformationTreatmentPlace> removePatientInformationTreatmentPlaces = new List<PatientInformationTreatmentPlace>();
                    foreach (var item in patient.PatientInformation.PatientInformationTreatmentPlaces)
                    {
                        if (model.PatientInformation.TreatmentPlaces.FirstOrDefault(x => x == item.TreatmentPlace.City) == null)
                        {
                            removePatientInformationTreatmentPlaces.Add(item);
                        }
                    }
                    patient.PatientInformation.PatientInformationTreatmentPlaces = patient.PatientInformation.PatientInformationTreatmentPlaces.Except(removePatientInformationTreatmentPlaces).ToList();

                    //Add new Treatment Place
                    foreach (var item in model.PatientInformation.TreatmentPlaces)
                    {
                        TreatmentPlace treatmentPlace = int.TryParse(item, out num) ?
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
                if (model.PatientInformation.Medicines.Count == 0)
                {
                    patient.PatientInformation.PatientInformationMedicines.Clear();
                }
                else
                {

                    //Remove not selected of database
                    List<PatientInformationMedicine> removePatientInformationMedicines = new List<PatientInformationMedicine>();
                    foreach (var item in patient.PatientInformation.PatientInformationMedicines)
                    {
                        if (model.PatientInformation.TreatmentPlaces.FirstOrDefault(x => x == item.Medicine.Name) == null)
                        {
                            removePatientInformationMedicines.Add(item);
                        }
                    }
                    patient.PatientInformation.PatientInformationMedicines = patient.PatientInformation.PatientInformationMedicines.Except(removePatientInformationMedicines).ToList();

                    //Added Medicine to Patient Information
                    foreach (var item in model.PatientInformation.Medicines)
                    {
                        Medicine medicine = int.TryParse(item, out num) ?
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

                patient.LastUpdatedDate = DateTime.Now;
                patient.LastUserUpdate = user;

                patient.FirstName = model.FirstName;
                patient.Surname = model.Surname;
                patient.RG = model.RG;
                patient.CPF = model.CPF;
                patient.FamiliarityGroup = model.FamiliarityGroup;
                patient.Sex = model.Sex;
                patient.CivilState = model.CivilState;
                patient.DateOfBirth = model.DateOfBirth;
                patient.Naturality.City = model.Naturality.City;
                patient.Naturality.State = model.Naturality.State;
                patient.Naturality.Country = model.Naturality.Country;


                TaskResult result = await _patientService.UpdateAsync(patient);
                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

            model.SelectProfessions = _professionService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.ProfessionId.ToString()
            }).ToList();

            model.SelectDoctors = _doctorService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.DoctorId.ToString()
            }).ToList();

            model.SelectCancerTypes = _cancerTypeService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.CancerTypeId.ToString()
            }).ToList();

            model.SelectMedicines = _medicineService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.MedicineId.ToString()
            }).ToList();

            model.SelectTreatmentPlaces = _treatmentPlaceService.GetAllAsync().Result.Select(x => new SelectListItem
            {
                Text = x.City,
                Value = x.TreatmentPlaceId.ToString()
            }).ToList();

            return PartialView("_EditPatient", model);
        }

        public IActionResult DisablePatient(string id)
        {
            DisablePatientViewModel disablePatientViewModel = new DisablePatientViewModel {
                DateTime = DateTime.Now
            };
            return PartialView("_DisablePatient", disablePatientViewModel);
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> DisablePatient(string id, DisablePatientViewModel model)
        {
            if (!string.IsNullOrEmpty(id))
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.PatientInformation, x => x.PatientInformation.ActivePatient);
                Patient patient = await _patientService.FindByIdAsync(id, specification);
                if (patient != null)
                {
                    if(model.DisablePatientType == Globals.DisablePatientType.death)
                    {
                        patient.PatientInformation.ActivePatient.Death = true;
                        patient.PatientInformation.ActivePatient.DeathDate = model.DateTime;
                    }
                    else if (model.DisablePatientType == Globals.DisablePatientType.discharge)
                    {
                        patient.PatientInformation.ActivePatient.Discharge = true;
                        patient.PatientInformation.ActivePatient.DischargeDate = model.DateTime;
                    }

                    TaskResult result = await _patientService.DeleteAsync(patient);

                    if (result.Succeeded)
                    {
                        return StatusCode(200, "200");
                    }
                    ModelState.AddErrors(result);
                    return PartialView("_DeletePatient", patient.FirstName);
                }
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DetailsPatient(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            BaseSpecification<Patient> specification = new BaseSpecification<Patient>(
                   x => x.Naturality, x => x.Profession, x => x.Family, x => x.FileAttachments, x => x.PatientInformation,
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
            
            PatientShowViewModel patientViewModel = new PatientShowViewModel
            {
                PatientId = patient.PatientId,
                Name = patient.FirstName + " " + patient.Surname,
                CivilState = patient.CivilState,
                CPF = patient.CPF,
                DateOfBirth = patient.DateOfBirth,
                FamiliarityGroup = patient.FamiliarityGroup,
                Profession = patient.Profession?.Name,
                RG = patient.RG,
                Sex = patient.Sex,
                Family = patient.Family,
                Naturality = new NaturalityViewModel
                {
                    City = patient.Naturality.City,
                    Country = patient.Naturality.Country,
                    State = patient.Naturality.State
                },
                FileAttachments = patient.FileAttachments,
                PatientInformation = new PatientInformationViewModel
                {
                    CancerTypes = patient.PatientInformation.PatientInformationCancerTypes.Select(x => x.CancerType.Name).ToList(),
                    Doctors = patient.PatientInformation.PatientInformationDoctors.Select(x => x.Doctor.Name).ToList(),
                    Medicines = patient.PatientInformation.PatientInformationMedicines.Select(x => x.Medicine.Name).ToList(),
                    TreatmentPlaces = patient.PatientInformation.PatientInformationTreatmentPlaces.Select(x => x.TreatmentPlace.City).ToList(),
                }
            };

            return View(patientViewModel);
        }

        [HttpGet]
        public async Task<IActionResult> ActivePatient(string id)
        {
            Patient patient = await _patientService.FindByIdAsync(id, ignoreQueryFilter: true);
            return PartialView("_ActivePatient", $"{patient.FirstName} {patient.Surname}");
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> ActivePatient(string id, IFormCollection form)
        {
            if (!string.IsNullOrEmpty(id))
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.PatientInformation, x => x.PatientInformation.ActivePatient);
                Patient patient = await _patientService.FindByIdAsync(id, specification, true);
                if (patient != null)
                {
                    patient.PatientInformation.ActivePatient.Discharge = false;
                    TaskResult result = ((PatientStore)_patientService).ActivePatient(patient);
                    if (result.Succeeded)
                    {
                        return StatusCode(200, "200");
                    }
                    ModelState.AddErrors(result);
                    return PartialView("_ActivePatient", $"{patient.FirstName} {patient.Surname}");
                }
            }
            return RedirectToAction("Index");
        }

        #region Custom Methods

        public JsonResult IsCpfExist(string Cpf, int PatientId)
        {
            Patient patient = ((PatientStore)_patientService).FindByCpfAsync(Cpf, PatientId).Result;

            if (patient != null)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }
        }

        public JsonResult IsRgExist(string Rg, int PatientId)
        {
            Patient patient = ((PatientStore)_patientService).FindByRgAsync(Rg, PatientId).Result;

            if (patient != null)
            {
                return Json(false);
            }
            else
            {
                return Json(true);
            }

        }

        #endregion
    }
}