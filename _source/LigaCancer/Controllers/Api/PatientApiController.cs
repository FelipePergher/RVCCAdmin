﻿using System.Threading.Tasks;
using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Code.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Models.SearchModel;
using System.Linq;
using LigaCancer.Data.Store;
using System;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    public class PatientApiController : Controller
    {
        private readonly IDataTable<Patient> _patientDataTable;

        public PatientApiController(IDataTable<Patient> patientDataTable)
        {
            _patientDataTable = patientDataTable;
        }

        [HttpPost("~/api/Patient/GetPatientDataTableResponseAsync")]
        public async Task<IActionResult> GetPatientDataTableResponseAsync(DataTableOptions options, PatientSearchModel patientSearchModel)
        {
            try
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(
                       x => x.PatientInformation, x => x.Family,
                       x => x.PatientInformation.PatientInformationCancerTypes,
                       x => x.PatientInformation.PatientInformationDoctors,
                       x => x.PatientInformation.PatientInformationMedicines,
                       x => x.PatientInformation.PatientInformationTreatmentPlaces
                );

                specification.IncludeStrings.Add("PatientInformation.PatientInformationDoctors.Doctor");
                specification.IncludeStrings.Add("PatientInformation.PatientInformationMedicines.Medicine");
                specification.IncludeStrings.Add("PatientInformation.PatientInformationCancerTypes.CancerType");
                specification.IncludeStrings.Add("PatientInformation.PatientInformationTreatmentPlaces.TreatmentPlace");

                //Filters
                if (!string.IsNullOrEmpty(patientSearchModel.CivilState))
                {
                    Globals.CivilState civilStateValue = (Globals.CivilState)int.Parse(patientSearchModel.CivilState);
                    specification.Wheres.Add(x => x.CivilState == civilStateValue);
                }

                if (!string.IsNullOrEmpty(patientSearchModel.Sex))
                {
                    Globals.Sex sexValue = (Globals.Sex)int.Parse(patientSearchModel.Sex);
                    specification.Wheres.Add(x => x.Sex == sexValue);
                }

                foreach (string item in patientSearchModel.CancerTypes)
                {
                    specification.Wheres.Add(x => x.PatientInformation.PatientInformationCancerTypes
                        .FirstOrDefault(y => y.CancerTypeId == int.Parse(item)) != null);
                }

                foreach (string item in patientSearchModel.TreatmentPlaces)
                {
                    specification.Wheres.Add(x => x.PatientInformation.PatientInformationTreatmentPlaces
                        .FirstOrDefault(y => y.TreatmentPlaceId == int.Parse(item)) != null);
                }

                foreach (string item in patientSearchModel.Doctors)
                {
                        specification.Wheres.Add(x => x.PatientInformation.PatientInformationDoctors
                            .FirstOrDefault(y => y.DoctorId == int.Parse(item)) != null);
                }

                foreach (string item in patientSearchModel.Medicines)
                {
                    specification.Wheres.Add(x => x.PatientInformation.PatientInformationMedicines
                        .FirstOrDefault(y => y.MedicineId == int.Parse(item)) != null);
                }

                if (!string.IsNullOrEmpty(patientSearchModel.FamiliarityGroup))
                {
                    bool result = bool.Parse(patientSearchModel.FamiliarityGroup);
                    if (result)
                    {
                        specification.Wheres.Add(x => x.FamiliarityGroup);
                    }
                    else
                    {
                        specification.Wheres.Add(x => !x.FamiliarityGroup);
                    }
                }

                if (patientSearchModel.Death)
                {
                    specification.Wheres.Add(x => x.PatientInformation.ActivePatient.Death);
                    return Ok(await  ((PatientStore)_patientDataTable).GetOptionResponseWithSpec(options, specification));
                }

                if (patientSearchModel.Discharge)
                {
                    specification.Wheres.Add(x => x.PatientInformation.ActivePatient.Discharge);
                    return Ok(await  ((PatientStore)_patientDataTable).GetOptionResponseWithSpec(options, specification));
                }

                return Ok(await _patientDataTable.GetOptionResponseWithSpec(options, specification));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
