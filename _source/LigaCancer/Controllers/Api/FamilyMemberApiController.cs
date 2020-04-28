using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Data.Repositories;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [ApiController]
    public class FamilyMemberApiController : Controller
    {
        private readonly IDataRepository<FamilyMember> _familyMemberService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<FamilyMemberApiController> _logger;

        public FamilyMemberApiController(IDataRepository<FamilyMember> familyMemberService, IDataRepository<Patient> patientService, ILogger<FamilyMemberApiController> logger)
        {
            _familyMemberService = familyMemberService;
            _patientService = patientService;
            _logger = logger;
        }

        [Authorize(Roles = Roles.AdminUserSocialAssistanceAuthorize)]
        [HttpPost("~/api/familyMember/search")]
        public async Task<IActionResult> FamilyMemberSearch([FromForm] SearchModel searchModel, [FromForm] FamilyMemberSearchModel familyMemberSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                List<FamilyMember> familyMembers = await _familyMemberService.GetAllAsync(null, sortColumn, sortDirection, familyMemberSearch);
                IEnumerable<FamilyMemberViewModel> data = familyMembers.Select(x => new FamilyMemberViewModel
                {
                    Name = x.Name,
                    Kinship = x.Kinship,
                    DateOfBirth = x.DateOfBirth.HasValue ? x.DateOfBirth.Value.ToString("dd/MM/yyyy") : "",
                    Sex = Globals.GetDisplayName(x.Sex),
                    MonthlyIncome = x.MonthlyIncome.ToString("C2"),
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                Patient patient = await _patientService.FindByIdAsync(familyMemberSearch.PatientId);
                string familyIncome = (familyMembers.Sum(x => x.MonthlyIncome) + patient.MonthlyIncome).ToString("C2");
                string perCapitaIncome = ((PatientRepository)_patientService).GetPerCapitaIncome(familyMembers, patient.MonthlyIncome);

                int recordsTotal = familyMembers.Count;

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered = recordsTotal, perCapitaIncome, familyIncome, monthlyIncome = patient.MonthlyIncome.ToString("C2") });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Family Member Search Error", null);
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(FamilyMember familyMember)
        {
            string editFamilyMember = string.Empty;
            string deleteFamilyMember = string.Empty;

            if (!User.IsInRole(Roles.SocialAssistance))
            {
                editFamilyMember = $"<a href='/FamilyMember/EditFamilyMember/{familyMember.FamilyMemberId}' data-toggle='modal' " +
                                   $"data-target='#modal-action' data-title='Editar Membro Familiar' class='dropdown-item editFamilyMemberButton'><i class='fas fa-edit'></i> Editar </a>";

                deleteFamilyMember = $"<a href='javascript:void(0);' data-url='/FamilyMember/DeleteFamilyMember' data-id='{familyMember.FamilyMemberId}' class='dropdown-item deleteFamilyMemberButton'>" +
                    $"<i class='fas fa-trash-alt'></i> Excluir </a>";
            }

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {editFamilyMember}" +
                $"      {deleteFamilyMember}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        #endregion

    }
}
