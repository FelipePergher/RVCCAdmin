// <copyright file="PatientExpenseTypeApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Data.Models.RelationModels;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [ApiController]
    public class PatientExpenseTypeApiController : Controller
    {
        private readonly IDataRepository<PatientExpenseType> _patientExpenseTypeService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<PatientExpenseTypeApiController> _logger;

        public PatientExpenseTypeApiController(IDataRepository<PatientExpenseType> patientExpenseTypeService, IDataRepository<Patient> patientService, ILogger<PatientExpenseTypeApiController> logger)
        {
            _patientExpenseTypeService = patientExpenseTypeService;
            _patientService = patientService;
            _logger = logger;
        }

        [HttpPost("~/api/patientExpenseType/search")]
        public async Task<IActionResult> PatientExpenseTypeSearch([FromForm] SearchModel searchModel, [FromForm] PatientExpenseTypeSearchModel patientExpenseTypeSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                List<PatientExpenseType> patientExpenseTypes = await _patientExpenseTypeService.GetAllAsync(new[] { nameof(PatientExpenseType.ExpenseType) }, sortColumn, sortDirection, patientExpenseTypeSearch);
                IEnumerable<PatientExpenseTypeViewModel> data = patientExpenseTypes.Select(x => new PatientExpenseTypeViewModel
                {
                    ExpenseType = x.ExpenseType.Name,
                    Frequency = Enums.GetDisplayName(x.ExpenseType.ExpenseTypeFrequency),
                    Value = x.Value.ToString("N2"),
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                Patient patient = await _patientService.FindByIdAsync(patientExpenseTypeSearch.PatientId);
                int recordsTotal = patientExpenseTypes.Count;

                return Ok(new
                {
                    searchModel.Draw,
                    data,
                    recordsTotal,
                    recordsFiltered = recordsTotal
                });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Family Member Search Error");
                return BadRequest();
            }
        }

        #region Private Methods

        private static string GetActionsHtml(PatientExpenseType patientExpenseType)
        {
            string options = $"<a href='/PatientExpenseType/EditPatientExpenseType/{patientExpenseType.PatientExpenseTypeId}' data-toggle='modal' " +
                             "data-target='#modal-action' data-title='Editar Despesa' class='dropdown-item editPatientExpenseTypeButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $"<a href='javascript:void(0);' data-url='/PatientExpenseType/DeletePatientExpenseType' data-id='{patientExpenseType.PatientExpenseTypeId}' class='dropdown-item deletePatientExpenseTypeButton'>" +
                       "<span class='fas fa-trash-alt'></span> Excluir </a>";

            string actionsHtml =
                "<div class='dropdown'>" +
                "  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                "  <div class='dropdown-menu'>" +
                $"      {options}" +
                "  </div>" +
                "</div>";

            return actionsHtml;
        }

        #endregion

    }
}
