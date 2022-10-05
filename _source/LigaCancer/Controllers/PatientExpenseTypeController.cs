// <copyright file="PatientExpenseTypeController.cs" company="Doffs">
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
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class PatientExpenseTypeController : Controller
    {
        private readonly IDataRepository<ExpenseType> _expenseTypeService;
        private readonly IDataRepository<PatientExpenseType> _patientExpenseTypeService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<PatientExpenseTypeController> _logger;

        public PatientExpenseTypeController(
            IDataRepository<ExpenseType> expenseTypeService,
            IDataRepository<PatientExpenseType> patientExpenseTypeService,
            IDataRepository<Patient> patientService,
            ILogger<PatientExpenseTypeController> logger)
        {
            _expenseTypeService = expenseTypeService;
            _patientExpenseTypeService = patientExpenseTypeService;
            _patientService = patientService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> AddPatientExpenseTypeAsync(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            var expenseTypes = await SelectHelper.GetExpenseTypeSelectAsync(_expenseTypeService, int.Parse(id));
            return PartialView("Partials/_AddPatientExpenseType", new PatientExpenseTypeFormModel { ExpenseTypes = expenseTypes });
        }

        [HttpPost]
        public async Task<IActionResult> AddPatientExpenseType(string id, PatientExpenseTypeFormModel patientExpenseTypeForm)
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

                List<ExpenseType> expenseTypesAllowed = await ((ExpenseTypeRepository)_expenseTypeService).GetNotRelatedToPatient(int.Parse(id));
                if (expenseTypesAllowed.All(x => x.ExpenseTypeId != int.Parse(patientExpenseTypeForm.ExpenseType)))
                {
                    return BadRequest("Tipo de despesa ja cadastrada para o paciente");
                }

                var patientExpenseType = new PatientExpenseType
                {
                    PatientId = int.Parse(id),
                    ExpenseTypeId = int.Parse(patientExpenseTypeForm.ExpenseType),
                    Value = double.TryParse(patientExpenseTypeForm.Value, out double value) ? value : 0
                };

                TaskResult result = await _patientExpenseTypeService.CreateAsync(patientExpenseType);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddPatientExpenseType", patientExpenseTypeForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditPatientExpenseType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            PatientExpenseType patientExpenseType = await _patientExpenseTypeService.FindByIdAsync(id);

            if (patientExpenseType == null)
            {
                return NotFound();
            }

            return PartialView("Partials/_EditPatientExpenseType", new PatientExpenseTypeFormModel
            {
                ExpenseType = patientExpenseType.ExpenseTypeId.ToString(),
                Value = patientExpenseType.Value.ToString("N2"),
                ExpenseTypes = await SelectHelper.GetExpenseTypeSelectAsync(_expenseTypeService, patientExpenseType.PatientId, patientExpenseType.ExpenseTypeId)
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditPatientExpenseType(string id, PatientExpenseTypeFormModel patientExpenseTypeForm)
        {
            if (ModelState.IsValid)
            {
                PatientExpenseType patientExpenseType = await _patientExpenseTypeService.FindByIdAsync(id);

                if (patientExpenseType == null)
                {
                    return NotFound();
                }

                List<ExpenseType> expenseTypesAllowed = await ((ExpenseTypeRepository)_expenseTypeService).GetNotRelatedToPatient(patientExpenseType.PatientId, patientExpenseType.ExpenseTypeId);
                if (expenseTypesAllowed.All(x => x.ExpenseTypeId != int.Parse(patientExpenseTypeForm.ExpenseType)))
                {
                    return BadRequest("Tipo de despesa ja cadastrada para o paciente");
                }

                patientExpenseType.ExpenseTypeId = int.Parse(patientExpenseTypeForm.ExpenseType);
                patientExpenseType.Value = double.TryParse(patientExpenseTypeForm.Value, out double value) ? value : 0;

                TaskResult result = await _patientExpenseTypeService.UpdateAsync(patientExpenseType);

                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditPatientExpenseType", patientExpenseTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeletePatientExpenseType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            PatientExpenseType patientExpenseType = await _patientExpenseTypeService.FindByIdAsync(id);

            if (patientExpenseType == null)
            {
                return NotFound();
            }

            TaskResult result = await _patientExpenseTypeService.DeleteAsync(patientExpenseType);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest();
        }
    }
}
