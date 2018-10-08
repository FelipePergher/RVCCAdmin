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

                Profession profession = await _professionService.FindByIdAsync(model.Profession);
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
                    CancerType cancerType = await _cancerTypeService.FindByIdAsync(item);
                    if(cancerType == null)
                    {
                        //Todo: salvar no bd o novo nome
                    }
                    patient.PatientInformation.PatientInformationCancerTypes.Add(new PatientInformationCancerType
                    {
                        CancerType = cancerType
                    });
                }

                //Added Doctor to Patient Information
                foreach (var item in model.PatientInformation.Doctors)
                {
                    Doctor doctor = await _doctorService.FindByIdAsync(item);
                    if (doctor == null)
                    {
                        //Todo: salvar no bd o novo nome
                    }
                    patient.PatientInformation.PatientInformationDoctors.Add(new PatientInformationDoctor
                    {
                        Doctor = doctor
                    });
                }

                //Added Treatment Place to Patient Information
                foreach (var item in model.PatientInformation.TreatmentPlaces)
                {
                    TreatmentPlace treatmentPlace = await _treatmentPlaceService.FindByIdAsync(item);
                    if (treatmentPlace == null)
                    {
                        //Todo: salvar no bd o novo nome
                    }
                    patient.PatientInformation.PatientInformationTreatmentPlaces.Add(new PatientInformationTreatmentPlace
                    {
                        TreatmentPlace = treatmentPlace
                    });
                }

                //Added Medicine to Patient Information
                foreach (var item in model.PatientInformation.Medicines)
                {
                    Medicine medicine = await _medicineService.FindByIdAsync(item);
                    if (medicine == null)
                    {
                        //Todo: salvar no bd o novo nome
                    }
                    patient.PatientInformation.PatientInformationMedicines.Add(new PatientInformationMedicine
                    {
                        Medicine = medicine
                    });
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
            PatientViewModel patientViewModel = new PatientViewModel();

            if (!string.IsNullOrEmpty(id))
            {
                Patient patient = await _patientService.FindByIdAsync(id);
                if(patient != null)
                {
                    patientViewModel = new PatientViewModel
                    {
                        PatientId = patient.PatientId,
                        FirstName = patient.FirstName
                    };
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
                Patient patient = await _patientService.FindByIdAsync(id);
                ApplicationUser user = await _userManager.GetUserAsync(this.User);

                patient.FirstName = model.FirstName;
                patient.LastUpdatedDate = DateTime.Now;
                patient.LastUserUpdate = user;

                TaskResult result = await _patientService.UpdateAsync(patient);
                if (result.Succeeded)
                {
                    return StatusCode(200, "200");
                }
                ModelState.AddErrors(result);
            }

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
                    name = patient.FirstName;
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

            var patient = await _patientService.FindByIdAsync(id.ToString());
            if (patient == null)
            {
                return NotFound();
            }

            return View(patient);
        }

        #region Custom Methods

        public JsonResult IsNameExist(string Name, int PatientId)
        {
            Patient patient = ((PatientStore)_patientService).FindByNameAsync(Name, PatientId).Result;

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
