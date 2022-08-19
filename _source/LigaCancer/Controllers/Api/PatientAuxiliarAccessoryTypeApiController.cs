// <copyright file="PatientAuxiliarAccessoryTypeApiController.cs" company="Doffs">
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
    public class PatientAuxiliarAccessoryTypeApiController : Controller
    {
        private readonly IDataRepository<PatientAuxiliarAccessoryType> _patientAuxiliarAccessoryTypeService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<PatientAuxiliarAccessoryTypeApiController> _logger;

        public PatientAuxiliarAccessoryTypeApiController(IDataRepository<PatientAuxiliarAccessoryType> patientAuxiliarAccessoryTypeService, IDataRepository<Patient> patientService, ILogger<PatientAuxiliarAccessoryTypeApiController> logger)
        {
            _patientAuxiliarAccessoryTypeService = patientAuxiliarAccessoryTypeService;
            _patientService = patientService;
            _logger = logger;
        }

        [HttpPost("~/api/patientAuxiliarAccessoryType/search")]
        public async Task<IActionResult> PatientAuxiliarAccessoryTypeSearch([FromForm] SearchModel searchModel, [FromForm] PatientAuxiliarAccessoryTypeSearchModel patientAuxiliarAccessoryTypeSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                List<PatientAuxiliarAccessoryType> patientAuxiliarAccessoryTypes = await _patientAuxiliarAccessoryTypeService.GetAllAsync(new[] { nameof(PatientAuxiliarAccessoryType.AuxiliarAccessoryType) }, sortColumn, sortDirection, patientAuxiliarAccessoryTypeSearch);
                IEnumerable<PatientAuxiliarAccessoryTypeViewModel> data = patientAuxiliarAccessoryTypes.Select(x => new PatientAuxiliarAccessoryTypeViewModel
                {
                    AuxiliarAccessoryType = x.AuxiliarAccessoryType.Name,
                    AuxiliarAccessoryTypeTime = Enums.GetDisplayName(x.AuxiliarAccessoryTypeTime),
                    Note = x.Note,
                    DuoDate = x.DuoDate != DateTime.MinValue ? x.DuoDate.ToDateString() : "-",
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                Patient patient = await _patientService.FindByIdAsync(patientAuxiliarAccessoryTypeSearch.PatientId);
                int recordsTotal = patientAuxiliarAccessoryTypes.Count;

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

        private static string GetActionsHtml(PatientAuxiliarAccessoryType patientAuxiliarAccessoryType)
        {
            string options = $"<a href='/PatientAuxiliarAccessoryType/EditPatientAuxiliarAccessoryType/{patientAuxiliarAccessoryType.PatientAuxiliarAccessoryTypeId}' data-toggle='modal' " +
                             "data-target='#modal-action' data-title='Editar Acessório Auxiliar' class='dropdown-item editPatientAuxiliarAccessoryTypeButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $"<a href='javascript:void(0);' data-url='/PatientAuxiliarAccessoryType/DeletePatientAuxiliarAccessoryType' data-id='{patientAuxiliarAccessoryType.PatientAuxiliarAccessoryTypeId}' class='dropdown-item deletePatientAuxiliarAccessoryTypeButton'>" +
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
