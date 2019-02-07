using System.Threading.Tasks;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Models.SearchModel;
using System.Linq;
using System.Collections.Generic;
using LigaCancer.Models.ViewModel;

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

        [HttpPost("~/api/Patient/search")]
        public async Task<IActionResult> GetPatientDataTableResponseAsync([FromForm] SearchModel searchModel, PatientSearchModel patientSearchModel)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<Patient> patients = await _patientService.GetAllAsync(new string[] { "PatientInformation" }, sortColumn, sortDirection);
                IEnumerable<PatientViewModel> data = patients.Select(x => new PatientViewModel
                {
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
            string actionsHtml = "";

            //string actionsHtml = $"<a href='/TreatmentPlace/EditTreatmentPlace/{treatmentPlace.TreatmentPlaceId}' data-toggle='modal' data-target='#modal-action' class='btn btn-secondary editTreatmentPlaceButton'><i class='fas fa-edit'></i> Editar </a>";

            //actionsHtml += $"<a href='javascript:void(0);' data-url='/TreatmentPlace/DeleteTreatmentPlace/{treatmentPlace.TreatmentPlaceId}' class='btn btn-danger ml-1 deleteTreatmentPlaceButton'><i class='fas fa-trash-alt'></i> Excluir </a>";

            return actionsHtml;
        }

        #endregion
    }
}
