// <copyright file="PatientTreatmentTypeApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
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
    public class PatientTreatmentTypeApiController : Controller
    {
        private readonly IDataRepository<PatientTreatmentType> _patientTreatmentTypeService;
        private readonly ILogger<PatientTreatmentTypeApiController> _logger;

        public PatientTreatmentTypeApiController(IDataRepository<PatientTreatmentType> patientTreatmentTypeService, ILogger<PatientTreatmentTypeApiController> logger)
        {
            _patientTreatmentTypeService = patientTreatmentTypeService;
            _logger = logger;
        }

        [HttpPost("~/api/patientTreatmentType/search")]
        public async Task<IActionResult> PatientTreatmentTypeSearch([FromForm] SearchModel searchModel, [FromForm] PatientTreatmentTypeSearchModel patientTreatmentTypeSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                List<PatientTreatmentType> patientTreatmentTypes = await _patientTreatmentTypeService.GetAllAsync(new[] { nameof(PatientTreatmentType.TreatmentType) }, sortColumn, sortDirection, patientTreatmentTypeSearch);
                IEnumerable<PatientTreatmentTypeViewModel> data = patientTreatmentTypes.Select(x => new PatientTreatmentTypeViewModel
                {
                    TreatmentType = x.TreatmentType.Name,
                    StartDate = x.StartDate.HasValue ? x.StartDate.Value.ToDateString() : string.Empty,
                    EndDate = x.EndDate.HasValue ? x.EndDate.Value.ToDateString() : string.Empty,
                    Note = x.Note,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = patientTreatmentTypes.Count;

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
                _logger.LogError(LogEvents.ListItems, e, "Patient Treatment Type Search Error");
                return BadRequest();
            }
        }

        #region Private Methods

        private static string GetActionsHtml(PatientTreatmentType patientTreatmentType)
        {
            string options = $"<a href='/PatientTreatmentType/EditPatientTreatmentType/{patientTreatmentType.PatientTreatmentTypeId}' data-toggle='modal' " +
                             "data-target='#modal-action' data-title='Editar Despesa' class='dropdown-item editPatientTreatmentTypeButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $"<a href='javascript:void(0);' data-url='/PatientTreatmentType/DeletePatientTreatmentType' data-id='{patientTreatmentType.PatientTreatmentTypeId}' class='dropdown-item deletePatientTreatmentTypeButton'>" +
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
