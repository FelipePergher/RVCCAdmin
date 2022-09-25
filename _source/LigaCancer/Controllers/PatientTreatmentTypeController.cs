// <copyright file="PatientTreatmentTypeController.cs" company="Doffs">
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class PatientTreatmentTypeController : Controller
    {
        private readonly IDataRepository<TreatmentType> _treatmentTypeService;
        private readonly IDataRepository<PatientTreatmentType> _patientTreatmentTypeService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<PatientTreatmentTypeController> _logger;

        public PatientTreatmentTypeController(
            IDataRepository<TreatmentType> treatmentTypeService,
            IDataRepository<PatientTreatmentType> patientTreatmentTypeService,
            IDataRepository<Patient> patientService,
            ILogger<PatientTreatmentTypeController> logger)
        {
            _treatmentTypeService = treatmentTypeService;
            _patientTreatmentTypeService = patientTreatmentTypeService;
            _patientService = patientService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> AddPatientTreatmentTypeAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var treatmentTypes = await SelectHelper.GetTreatmentTypeSelectAsync(_treatmentTypeService, int.Parse(id));
            return PartialView("Partials/_AddPatientTreatmentType", new PatientTreatmentTypeFormModel { TreatmentTypes = treatmentTypes });
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientTreatmentType(string id, PatientTreatmentTypeFormModel patientTreatmentTypeForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                if (await _patientService.FindByIdAsync(id) == null)
                {
                    return NotFound();
                }

                List<TreatmentType> treatmentTypesAllowed = await ((TreatmentTypeRepository)_treatmentTypeService).GetNotRelatedToPatient(int.Parse(id));
                if (treatmentTypesAllowed.All(x => x.TreatmentTypeId != int.Parse(patientTreatmentTypeForm.TreatmentType)))
                {
                    return BadRequest("Tipo de tratamento ja cadastrada para o paciente");
                }

                var patientTreatmentType = new PatientTreatmentType
                {
                    PatientId = int.Parse(id),
                    TreatmentTypeId = int.Parse(patientTreatmentTypeForm.TreatmentType),
                    StartDate = !string.IsNullOrEmpty(patientTreatmentTypeForm.StartDate) ? DateTime.Parse(patientTreatmentTypeForm.StartDate) : null,
                    EndDate = !string.IsNullOrEmpty(patientTreatmentTypeForm.EndDate) ? DateTime.Parse(patientTreatmentTypeForm.EndDate) : null,
                    Note = patientTreatmentTypeForm.Note
                };

                TaskResult result = await _patientTreatmentTypeService.CreateAsync(patientTreatmentType);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddPatientTreatmentType", patientTreatmentTypeForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditPatientTreatmentType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            PatientTreatmentType patientTreatmentType = await _patientTreatmentTypeService.FindByIdAsync(id);

            if (patientTreatmentType == null)
            {
                return NotFound();
            }

            return PartialView("Partials/_EditPatientTreatmentType", new PatientTreatmentTypeFormModel
            {
                TreatmentType = patientTreatmentType.TreatmentTypeId.ToString(),
                TreatmentTypes = await SelectHelper.GetTreatmentTypeSelectAsync(_treatmentTypeService, patientTreatmentType.PatientId, patientTreatmentType.TreatmentTypeId),
                StartDate = patientTreatmentType.StartDate.HasValue ? patientTreatmentType.StartDate.Value.ToDateString() : string.Empty,
                EndDate = patientTreatmentType.EndDate.HasValue ? patientTreatmentType.EndDate.Value.ToDateString() : string.Empty,
                Note = patientTreatmentType.Note,
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientTreatmentType(string id, PatientTreatmentTypeFormModel patientTreatmentTypeForm)
        {
            if (ModelState.IsValid)
            {
                PatientTreatmentType patientTreatmentType = await _patientTreatmentTypeService.FindByIdAsync(id);

                if (patientTreatmentType == null)
                {
                    return NotFound();
                }

                List<TreatmentType> treatmentTypesAllowed = await ((TreatmentTypeRepository)_treatmentTypeService).GetNotRelatedToPatient(patientTreatmentType.PatientId, patientTreatmentType.TreatmentTypeId);
                if (treatmentTypesAllowed.All(x => x.TreatmentTypeId != int.Parse(patientTreatmentTypeForm.TreatmentType)))
                {
                    return BadRequest("Tipo de tratamento ja cadastrada para o paciente");
                }

                patientTreatmentType.TreatmentTypeId = int.Parse(patientTreatmentTypeForm.TreatmentType);
                patientTreatmentType.StartDate = !string.IsNullOrEmpty(patientTreatmentTypeForm.StartDate) ? DateTime.Parse(patientTreatmentTypeForm.StartDate) : null;
                patientTreatmentType.EndDate = !string.IsNullOrEmpty(patientTreatmentTypeForm.EndDate) ? DateTime.Parse(patientTreatmentTypeForm.EndDate) : null;
                patientTreatmentType.Note = patientTreatmentTypeForm.Note;

                TaskResult result = await _patientTreatmentTypeService.UpdateAsync(patientTreatmentType);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditPatientTreatmentType", patientTreatmentTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePatientTreatmentType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            PatientTreatmentType patientTreatmentType = await _patientTreatmentTypeService.FindByIdAsync(id);

            if (patientTreatmentType == null)
            {
                return NotFound();
            }

            TaskResult result = await _patientTreatmentTypeService.DeleteAsync(patientTreatmentType);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}
