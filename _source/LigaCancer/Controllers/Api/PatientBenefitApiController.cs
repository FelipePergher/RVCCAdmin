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
using RVCC.Data.Repositories;

namespace RVCC.Controllers.Api
{
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

        [Authorize(Roles = Roles.AdminUserSocialAssistanceAuthorize)]
        [HttpPost("~/api/patientBenefit/search")]
        public async Task<IActionResult> PatientBenefitSearch([FromForm] SearchModel searchModel, [FromForm] PatientBenefitSearchModel patientBenefitSearchModel)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<PatientBenefit> patientBenefits = await _patientBenefitService.GetAllAsync(new[] { "Patient", "Benefit" }, sortColumn, sortDirection, patientBenefitSearchModel);
                IEnumerable<PatientBenefitViewModel> data = patientBenefits.Select(x => new PatientBenefitViewModel
                {
                    Patient = $"{x.Patient.FirstName} {x.Patient.Surname}",
                    Date = x.BenefitDate.ToString("dd/MM/yyyy"),
                    Benefit = x.Benefit.Name,
                    Quantity = x.Quantity.ToString(),
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = string.IsNullOrEmpty(patientBenefitSearchModel.PatientId) 
                    ? _patientBenefitService.Count() 
                    : ((PatientBenefitRepository)_patientBenefitService).CountByPatient(int.Parse(patientBenefitSearchModel.PatientId));
                int recordsFiltered = patientBenefits.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "PatientBenefit Search Error", null);
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(PatientBenefit patientBenefit)
        {
            string editPatientBenefit = string.Empty;
            string deletePatientBenefit = string.Empty;
            if (!User.IsInRole(Roles.SocialAssistance))
            {
                editPatientBenefit = $"<a href='/PatientBenefit/EditPatientBenefit/{patientBenefit.PatientId}/{patientBenefit.BenefitId}' data-toggle='modal' data-target='#modal-action' " +
                   "data-title='Editar Benefício de Paciente ' class='dropdown-item editPatientBenefitButton'><i class='fas fa-edit'></i> Editar </a>";

                deletePatientBenefit = $@"<a href='javascript:void(0);' data-url='/PatientBenefit/DeletePatientBenefit' data-patientId='{patientBenefit.PatientId}' 
                    data-benefitId='{patientBenefit.BenefitId}' class='deletePatientBenefitButton dropdown-item'><i class='fas fa-trash-alt'></i> Excluir </a>";
            }

            string actionsHtml =
                $@"<div class='dropdown'>
                  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>
                  <div class='dropdown-menu'>
                      {editPatientBenefit}
                      {deletePatientBenefit}
                  </div>
                </div>";

            return actionsHtml;
        }

        #endregion
    }
}
