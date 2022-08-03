// <copyright file="PatientBenefitApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.RelationModels;
using RVCC.Data.Repositories;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [ApiController]
    public class PatientBenefitApiController : Controller
    {
        private readonly IDataRepository<PatientBenefit> _patientBenefitService;
        private readonly ILogger<PatientBenefitApiController> _logger;

        public PatientBenefitApiController(IDataRepository<PatientBenefit> patientBenefitService, ILogger<PatientBenefitApiController> logger)
        {
            _patientBenefitService = patientBenefitService;
            _logger = logger;
        }

        [HttpPost("~/api/patientBenefit/search")]
        public async Task<IActionResult> PatientBenefitSearch([FromForm] SearchModel searchModel, [FromForm] PatientBenefitSearchModel patientBenefitSearchModel)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<PatientBenefit> patientBenefits = await _patientBenefitService.GetAllAsync(new[] { nameof(PatientBenefit.Patient), nameof(PatientBenefit.Benefit) }, sortColumn, sortDirection, patientBenefitSearchModel);
                IEnumerable<PatientBenefitViewModel> data = patientBenefits.Select(x => new PatientBenefitViewModel
                {
                    Patient = $"{x.Patient.FirstName} {x.Patient.Surname}",
                    Date = x.BenefitDate.ToDateString(),
                    Benefit = x.Benefit.Name,
                    Quantity = x.Quantity.ToString(),
                    Actions = GetActionsHtml(x, User)
                }).Skip(skip).Take(take);

                int recordsTotal = string.IsNullOrEmpty(patientBenefitSearchModel.PatientId)
                    ? _patientBenefitService.Count()
                    : ((PatientBenefitRepository)_patientBenefitService).CountByPatient(int.Parse(patientBenefitSearchModel.PatientId));
                int recordsFiltered = patientBenefits.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "PatientBenefit Search Error");
                return BadRequest();
            }
        }

        #region Private Methods

        private static string GetActionsHtml(PatientBenefit patientBenefit, ClaimsPrincipal user)
        {
            string options = $"<a href='/PatientBenefit/EditPatientBenefit/{patientBenefit.PatientBenefitId}' data-toggle='modal' data-target='#modal-action' " +
                                                  "data-title='Editar Benefício de Paciente ' class='dropdown-item editPatientBenefitButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $@"<a href='javascript:void(0);' data-url='/PatientBenefit/DeletePatientBenefit' data-patientBenefitId='{patientBenefit.PatientBenefitId}' 
                    class='deletePatientBenefitButton dropdown-item'><span class='fas fa-trash-alt'></span> Excluir </a>";

            string actionsHtml =
                $@"<div class='dropdown'>
                  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>
                  <div class='dropdown-menu'>
                      {options}
                  </div>
                </div>";

            return actionsHtml;
        }

        #endregion
    }
}
