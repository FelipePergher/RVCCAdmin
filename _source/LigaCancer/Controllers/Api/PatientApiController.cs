using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.SearchModel;
using LigaCancer.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), ApiController]
    public class PatientApiController : Controller
    {
        private readonly IDataStore<Patient> _patientService;

        public PatientApiController(IDataStore<Patient> patientService)
        {
            _patientService = patientService;
        }

        [HttpPost("~/api/patient/search")]
        public async Task<IActionResult> PatientSearch([FromForm] SearchModel searchModel, [FromForm] PatientSearchModel patientSearchModel)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<Patient> patients = await _patientService.GetAllAsync(
                    new string[] { 
                        "PatientInformation", "Naturality", "PatientInformation.ActivePatient", "PatientInformation.PatientInformationDoctors", "PatientInformation.PatientInformationDoctors.Doctor",
                        "Family", "PatientInformation.PatientInformationCancerTypes", "PatientInformation.PatientInformationMedicines",
                        "PatientInformation.PatientInformationTreatmentPlaces", "PatientInformation.PatientInformationMedicines.Medicine",
                        "PatientInformation.PatientInformationCancerTypes.CancerType", "PatientInformation.PatientInformationTreatmentPlaces.TreatmentPlace"
                    }, 
                    sortColumn, sortDirection);
                IEnumerable<PatientViewModel> data = patients.Select(x => new PatientViewModel
                {
                    Status = x.PatientInformation.ActivePatient.Discharge ? "Alta" : x.PatientInformation.ActivePatient.Death ? "Óbito" : "Ativo",
                    FirstName = x.FirstName,
                    LastName = x.Surname,
                    Rg = x.RG,
                    Cpf = x.CPF,
                    DateOfBirth = x.DateOfBirth.ToString("dd/MM/yyyy"),
                    Sex = x.Sex.ToString(),
                    CivilState = x.CivilState.ToString(),
                    FamiliarityGroup = x.FamiliarityGroup ? "<span class='fa fa-check'></span>" : "",
                    Profession = x.Profession,
                    //FamilyIncome = $"${x.Family.FamilyIncome.ToString("N")}",
                    //PerCapitaIncome = $"${x.Family.PerCapitaIncome.ToString("N")}",
                    FamilyIncome = $"${x.Family.MonthlyIncome.ToString("N")}",
                    PerCapitaIncome = $"${x.Family.MonthlyIncome.ToString("N")}",
                    Medicines = string.Join(", ", x.PatientInformation.PatientInformationMedicines.Select(y => y.Medicine.Name).ToList()),
                    Canceres = string.Join(", ", x.PatientInformation.PatientInformationCancerTypes.Select(y => y.CancerType.Name).ToList()),
                    Doctors = string.Join(", ", x.PatientInformation.PatientInformationDoctors.Select(y => y.Doctor.Name).ToList()),
                    TreatmentPlaces = string.Join(", ", x.PatientInformation.PatientInformationTreatmentPlaces.Select(y => y.TreatmentPlace.City).ToList()),
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _patientService.Count();
                int recordsFiltered = patients.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch
            {
                return BadRequest();
            }

            //BaseSpecification<Patient> specification = new BaseSpecification<Patient>(
            //       x => x.PatientInformation, x => x.Family,
            //       x => x.PatientInformation.PatientInformationCancerTypes,
            //       x => x.PatientInformation.PatientInformationDoctors,
            //       x => x.PatientInformation.PatientInformationMedicines,
            //       x => x.PatientInformation.PatientInformationTreatmentPlaces
            //);

            //specification.IncludeStrings.Add("PatientInformation.PatientInformationDoctors.Doctor");
            //specification.IncludeStrings.Add("PatientInformation.PatientInformationMedicines.Medicine");
            //specification.IncludeStrings.Add("PatientInformation.PatientInformationCancerTypes.CancerType");
            //specification.IncludeStrings.Add("PatientInformation.PatientInformationTreatmentPlaces.TreatmentPlace");

            //Filters
            //if (!string.IsNullOrEmpty(patientSearchModel.CivilState))
            //{
            //    Globals.CivilState civilStateValue = (Globals.CivilState)int.Parse(patientSearchModel.CivilState);
            //    specification.Wheres.Add(x => x.CivilState == civilStateValue);
            //}

            //if (!string.IsNullOrEmpty(patientSearchModel.Sex))
            //{
            //    Globals.Sex sexValue = (Globals.Sex)int.Parse(patientSearchModel.Sex);
            //    specification.Wheres.Add(x => x.Sex == sexValue);
            //}

            //foreach (string item in patientSearchModel.CancerTypes)
            //{
            //    specification.Wheres.Add(x => x.PatientInformation.PatientInformationCancerTypes
            //        .FirstOrDefault(y => y.CancerTypeId == int.Parse(item)) != null);
            //}

            //foreach (string item in patientSearchModel.TreatmentPlaces)
            //{
            //    specification.Wheres.Add(x => x.PatientInformation.PatientInformationTreatmentPlaces
            //        .FirstOrDefault(y => y.TreatmentPlaceId == int.Parse(item)) != null);
            //}

            //foreach (string item in patientSearchModel.Doctors)
            //{
            //        specification.Wheres.Add(x => x.PatientInformation.PatientInformationDoctors
            //            .FirstOrDefault(y => y.DoctorId == int.Parse(item)) != null);
            //}

            //foreach (string item in patientSearchModel.Medicines)
            //{
            //    specification.Wheres.Add(x => x.PatientInformation.PatientInformationMedicines
            //        .FirstOrDefault(y => y.MedicineId == int.Parse(item)) != null);
            //}

            //if (!string.IsNullOrEmpty(patientSearchModel.FamiliarityGroup))
            //{
            //    bool result = bool.Parse(patientSearchModel.FamiliarityGroup);
            //    if (result)
            //    {
            //        specification.Wheres.Add(x => x.FamiliarityGroup);
            //    }
            //    else
            //    {
            //        specification.Wheres.Add(x => !x.FamiliarityGroup);
            //    }
            //}

            //if (patientSearchModel.Death)
            //{
            //    specification.Wheres.Add(x => x.PatientInformation.ActivePatient.Death);
            //    return Ok(await  ((PatientStore)_patientDataTable).GetOptionResponseWithSpec(options, specification));
            //}

            //if (patientSearchModel.Discharge)
            //{
            //    specification.Wheres.Add(x => x.PatientInformation.ActivePatient.Discharge);
            //    return Ok(await  ((PatientStore)_patientDataTable).GetOptionResponseWithSpec(options, specification));
            //}

        }

        #region Private Methods

        private string GetActionsHtml(Patient patient)
        {
            string options = string.Empty;
            if (!patient.PatientInformation.ActivePatient.Death && !patient.PatientInformation.ActivePatient.Discharge)
            {
                string editPatient = $"<a href='/Patient/PatientProfile/{patient.PatientId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Editar Paciente' class='dropdown-item editPatientButton'><i class='fas fa-edit'></i> Editar Paciente</a>";
                string editNaturality = $"<a href='/Patient/PatientNaturality/{(patient.Naturality != null ? patient.Naturality.NaturalityId : patient.PatientId)}" +
                    $"?isNaturalityId={patient.Naturality != null}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Editar Naturalidade' class='dropdown-item editNaturalityButton'><i class='fas fa-edit'></i> Editar Naturalidade</a>";
                string archivePatient = $"<a href='/Patient/ArchivePatient/{patient.PatientId}'' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Arquivar Paciente' class='archivePatientButton dropdown-item'><i class='fas fa-user-alt-slash'></i> Arquivar </a>";

                options = editPatient + editNaturality + archivePatient;
            }
            else
            {
                string enablePatient = $"<a href='javascript:void(0);' data-url='/Patient/ActivePatient' data-id='{patient.PatientId}' " +
                    $"data-title='Ativar Paciente' class='activePatientButton dropdown-item'><i class='fas fa-user-plus'></i> Reativar </a>";
                string deletePatient = $"<a href='javascript:void(0);' data-url='/Patient/DeletePatient' data-id='{patient.PatientId}' " +
                $"data-title='Deletar Paciente' class='deletePatientButton dropdown-item'><i class='fas fa-trash-alt'></i> Excluir </a>";

                if (!patient.PatientInformation.ActivePatient.Death)
                {
                    options = enablePatient;
                }

                options += deletePatient;
            }

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {options}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        #endregion
    }
}
