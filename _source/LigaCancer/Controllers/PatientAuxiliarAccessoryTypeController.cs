// <copyright file="PatientAuxiliarAccessoryTypeController.cs" company="Doffs">
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
    public class PatientAuxiliarAccessoryTypeController : Controller
    {
        private readonly IDataRepository<AuxiliarAccessoryType> _auxiliarAccessoryTypeService;
        private readonly IDataRepository<PatientAuxiliarAccessoryType> _patientAuxiliarAccessoryTypeService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<PatientAuxiliarAccessoryTypeController> _logger;

        public PatientAuxiliarAccessoryTypeController(
            IDataRepository<AuxiliarAccessoryType> auxiliarAccessoryTypeService,
            IDataRepository<PatientAuxiliarAccessoryType> patientAuxiliarAccessoryTypeService,
            IDataRepository<Patient> patientService,
            ILogger<PatientAuxiliarAccessoryTypeController> logger)
        {
            _auxiliarAccessoryTypeService = auxiliarAccessoryTypeService;
            _patientAuxiliarAccessoryTypeService = patientAuxiliarAccessoryTypeService;
            _patientService = patientService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> AddPatientAuxiliarAccessoryTypeAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var auxiliarAccessoryTypes = await SelectHelper.GetAuxiliarAccessoryTypeSelectAsync(_auxiliarAccessoryTypeService, int.Parse(id));
            return PartialView("Partials/_AddPatientAuxiliarAccessoryType", new PatientAuxiliarAccessoryTypeFormModel { AuxiliarAccessoryTypes = auxiliarAccessoryTypes });
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientAuxiliarAccessoryType(string id, PatientAuxiliarAccessoryTypeFormModel patientAuxiliarAccessoryTypeForm)
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

                List<AuxiliarAccessoryType> auxiliarAccessoryTypesAllowed = await ((AuxiliarAccessoryTypeRepository)_auxiliarAccessoryTypeService).GetNotRelatedToPatient(int.Parse(id));
                if (auxiliarAccessoryTypesAllowed.All(x => x.AuxiliarAccessoryTypeId != int.Parse(patientAuxiliarAccessoryTypeForm.AuxiliarAccessoryType)))
                {
                    return BadRequest("Tipo de Acessório Auxiliar ja cadastrado para o paciente");
                }

                var patientAuxiliarAccessoryType = new PatientAuxiliarAccessoryType
                {
                    PatientId = int.Parse(id),
                    AuxiliarAccessoryTypeId = int.Parse(patientAuxiliarAccessoryTypeForm.AuxiliarAccessoryType),
                    Note = patientAuxiliarAccessoryTypeForm.Note,
                    DuoDate = DateTime.TryParse(patientAuxiliarAccessoryTypeForm.DuoDate, out DateTime duoDate) ? duoDate : DateTime.MinValue,
                    AuxiliarAccessoryTypeTime = patientAuxiliarAccessoryTypeForm.AuxiliarAccessoryTypeTime,
                };

                TaskResult result = await _patientAuxiliarAccessoryTypeService.CreateAsync(patientAuxiliarAccessoryType);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddPatientAuxiliarAccessoryType", patientAuxiliarAccessoryTypeForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditPatientAuxiliarAccessoryType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            PatientAuxiliarAccessoryType patientAuxiliarAccessoryType = await _patientAuxiliarAccessoryTypeService.FindByIdAsync(id);

            if (patientAuxiliarAccessoryType == null)
            {
                return NotFound();
            }

            return PartialView("Partials/_EditPatientAuxiliarAccessoryType", new PatientAuxiliarAccessoryTypeFormModel
            {
                AuxiliarAccessoryType = patientAuxiliarAccessoryType.AuxiliarAccessoryTypeId.ToString(),
                Note = patientAuxiliarAccessoryType.Note,
                DuoDate = patientAuxiliarAccessoryType.DuoDate != DateTime.MinValue ? patientAuxiliarAccessoryType.DuoDate.ToDateString() : string.Empty,
                AuxiliarAccessoryTypeTime = patientAuxiliarAccessoryType.AuxiliarAccessoryTypeTime,
                AuxiliarAccessoryTypes = await SelectHelper.GetAuxiliarAccessoryTypeSelectAsync(_auxiliarAccessoryTypeService, patientAuxiliarAccessoryType.PatientId, patientAuxiliarAccessoryType.AuxiliarAccessoryTypeId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientAuxiliarAccessoryType(string id, PatientAuxiliarAccessoryTypeFormModel patientAuxiliarAccessoryTypeForm)
        {
            if (ModelState.IsValid)
            {
                PatientAuxiliarAccessoryType patientAuxiliarAccessoryType = await _patientAuxiliarAccessoryTypeService.FindByIdAsync(id);

                if (patientAuxiliarAccessoryType == null)
                {
                    return NotFound();
                }

                List<AuxiliarAccessoryType> auxiliarAccessoryTypesAllowed = await ((AuxiliarAccessoryTypeRepository)_auxiliarAccessoryTypeService).GetNotRelatedToPatient(patientAuxiliarAccessoryType.PatientId, patientAuxiliarAccessoryType.AuxiliarAccessoryTypeId);
                if (auxiliarAccessoryTypesAllowed.All(x => x.AuxiliarAccessoryTypeId != int.Parse(patientAuxiliarAccessoryTypeForm.AuxiliarAccessoryType)))
                {
                    return BadRequest("Tipo de Acessório Auxiliar ja cadastrado para o paciente");
                }

                patientAuxiliarAccessoryType.AuxiliarAccessoryTypeId = int.Parse(patientAuxiliarAccessoryTypeForm.AuxiliarAccessoryType);
                patientAuxiliarAccessoryType.Note = patientAuxiliarAccessoryTypeForm.Note;
                patientAuxiliarAccessoryType.DuoDate = DateTime.TryParse(patientAuxiliarAccessoryTypeForm.DuoDate, out DateTime duoDate) ? duoDate : DateTime.MinValue;
                patientAuxiliarAccessoryType.AuxiliarAccessoryTypeTime = patientAuxiliarAccessoryTypeForm.AuxiliarAccessoryTypeTime;

                TaskResult result = await _patientAuxiliarAccessoryTypeService.UpdateAsync(patientAuxiliarAccessoryType);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditPatientAuxiliarAccessoryType", patientAuxiliarAccessoryTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePatientAuxiliarAccessoryType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            PatientAuxiliarAccessoryType patientAuxiliarAccessoryType = await _patientAuxiliarAccessoryTypeService.FindByIdAsync(id);

            if (patientAuxiliarAccessoryType == null)
            {
                return NotFound();
            }

            TaskResult result = await _patientAuxiliarAccessoryTypeService.DeleteAsync(patientAuxiliarAccessoryType);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}
