using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using LigaCancer.Data;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.MedicalViewModels;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using LigaCancer.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using LigaCancer.Data.Models.ManyToManyModels;

namespace LigaCancer.Controllers
{
    //[Authorize]
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

        public async Task<IActionResult> Index(string sortOrder, string currentSearchNameFilter, string searchNameString, int? page)
        {
            IQueryable<Patient> patients = _patientService.GetAllQueryable();
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = string.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            if (searchNameString != null)
            {
                page = 1;
            }
            else
            {
                searchNameString = currentSearchNameFilter;
            }

            ViewData["CurrentSearchNameFilter"] = searchNameString;

            if (!string.IsNullOrEmpty(searchNameString))
            {
                patients = patients.Where(s => s.FirstName.Contains(searchNameString));
            }

            switch (sortOrder)
            {
                case "name_desc":
                    patients = patients.OrderByDescending(s => s.FirstName);
                    break;
                default:
                    patients = patients.OrderBy(s => s.FirstName);
                    break;
            }

            int pageSize = 4;

            PaginatedList<Patient> paginateList = await PaginatedList<Patient>.CreateAsync(patients.AsNoTracking(), page ?? 1, pageSize);
            return View(paginateList);
        }

        public IActionResult AddPatient()
        {
            PatientViewModel patientViewModel = new PatientViewModel
            {
                SelectProfessions = _professionService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.ProfessionId.ToString()
                }).ToList(),
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
                if (profession == null)
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
                else
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
                SelectProfessions = _professionService.GetAllAsync().Result.Select(x => new SelectListItem
                {
                    Text = x.Name,
                    Value = x.ProfessionId.ToString()
                }).ToList(),
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

            if (!string.IsNullOrEmpty(id))
            {
                Patient patient = await _patientService.FindByIdAsync(id, new string[] {
                    "Naturality", "PatientInformation", "Profession", "PatientInformation.PatientInformationDoctors", "PatientInformation.PatientInformationMedicines",
                    "PatientInformation.PatientInformationCancerTypes", "PatientInformation.PatientInformationTreatmentPlaces"
                });
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
                    patientViewModel.Profession = patient.Profession.ProfessionId.ToString();
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
                Patient patient = await _patientService.FindByIdAsync(id, new string[] {
                     "Naturality", "PatientInformation", "Profession",
                    "PatientInformation.PatientInformationDoctors", "PatientInformation.PatientInformationDoctors.Doctor",
                    "PatientInformation.PatientInformationMedicines", "PatientInformation.PatientInformationMedicines.Medicine",
                    "PatientInformation.PatientInformationCancerTypes", "PatientInformation.PatientInformationCancerTypes.CancerType",
                    "PatientInformation.PatientInformationTreatmentPlaces", "PatientInformation.PatientInformationTreatmentPlaces.TreatmentPlace"
                });
                ApplicationUser user = await _userManager.GetUserAsync(this.User);

                Profession profession = int.TryParse(model.Profession, out int num) ?
                    await _professionService.FindByIdAsync(model.Profession) : await ((ProfessionStore)_professionService).FindByNameAsync(model.Profession);
                if (profession == null)
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
                else
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

        public async Task<IActionResult> DeletePatient(string id)
        {
            string name = string.Empty;

            if (!string.IsNullOrEmpty(id))
            {
                Patient patient = await _patientService.FindByIdAsync(id);
                if (patient != null)
                {
                    name = patient.FirstName + " " + patient.Surname;
                }
            }

            return PartialView("_DeletePatient", name);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeletePatient(string id, IFormCollection form)
        {
            if (!string.IsNullOrEmpty(id))
            {
                Patient patient = await _patientService.FindByIdAsync(id);
                if (patient != null)
                {
                    patient.RG = DateTime.Now + "-" + patient.RG;
                    patient.CPF = DateTime.Now + "-" + patient.CPF;
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

            Patient patient = await _patientService.FindByIdAsync(id, new string[] {
                    "Naturality", "PatientInformation", "Profession", "Phones",
                    "PatientInformation.PatientInformationDoctors", "PatientInformation.PatientInformationDoctors.Doctor",
                    "PatientInformation.PatientInformationMedicines", "PatientInformation.PatientInformationMedicines.Medicine",
                    "PatientInformation.PatientInformationCancerTypes", "PatientInformation.PatientInformationCancerTypes.CancerType",
                    "PatientInformation.PatientInformationTreatmentPlaces", "PatientInformation.PatientInformationTreatmentPlaces.TreatmentPlace"
                });

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
                Profession = patient.Profession.Name,
                RG = patient.RG,
                Sex = patient.Sex,
                Phones = patient.Phones,
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

        public IActionResult AddAddress()
        {
            return PartialView("_AddAddress");
        }

        public IActionResult AddfamilyMember()
        {
            return PartialView("_AddFamilyMember");
        }

        #endregion
    }
}