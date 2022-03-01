// <copyright file="FamilyMemberApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [ApiController]
    public class FamilyMemberApiController : Controller
    {
        private readonly IDataRepository<FamilyMember> _familyMemberService;
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<FamilyMemberApiController> _logger;
        private readonly IDataRepository<AdminInfo> _adminInfoService;

        public FamilyMemberApiController(
            IDataRepository<FamilyMember> familyMemberService,
            IDataRepository<Patient> patientService,
            IDataRepository<AdminInfo> adminInfoService,
            ILogger<FamilyMemberApiController> logger)
        {
            _familyMemberService = familyMemberService;
            _patientService = patientService;
            _adminInfoService = adminInfoService;
            _logger = logger;
        }

        [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
        [HttpPost("~/api/familyMember/search")]
        public async Task<IActionResult> FamilyMemberSearch([FromForm] SearchModel searchModel, [FromForm] FamilyMemberSearchModel familyMemberSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                AdminInfo adminInfo = (await _adminInfoService.GetAllAsync()).FirstOrDefault();
                List<FamilyMember> familyMembers = await _familyMemberService.GetAllAsync(null, sortColumn, sortDirection, familyMemberSearch);
                IEnumerable<FamilyMemberViewModel> data = familyMembers.Select(x => new FamilyMemberViewModel
                {
                    Name = x.Name,
                    Kinship = x.Kinship,
                    DateOfBirth = x.DateOfBirth.HasValue ? x.DateOfBirth.Value.ToString("dd/MM/yyyy") : string.Empty,
                    Sex = Enums.GetDisplayName(x.Sex),
                    MonthlyIncome = $"{x.MonthlyIncomeMinSalary:N2} Sal. Mín. ({adminInfo.MinSalary * x.MonthlyIncomeMinSalary:C2})",
                    Actions = GetActionsHtml(x, User)
                }).Skip(skip).Take(take);

                Patient patient = await _patientService.FindByIdAsync(familyMemberSearch.PatientId);
                var familyIncomeDouble = (familyMembers.Sum(x => x.MonthlyIncomeMinSalary) + patient.MonthlyIncomeMinSalary) * adminInfo.MinSalary;
                string familyIncome = familyIncomeDouble.ToString("C2");
                string perCapitaIncome = (familyIncomeDouble / (familyMembers.Count + 1)).ToString("C2");

                int recordsTotal = familyMembers.Count;

                return Ok(new
                {
                    searchModel.Draw,
                    data,
                    recordsTotal,
                    recordsFiltered = recordsTotal,
                    perCapitaIncome,
                    familyIncome,
                    MonthlyIncome = $"{patient.MonthlyIncomeMinSalary:N2} Sal. Mín. ({adminInfo.MinSalary * patient.MonthlyIncomeMinSalary:C2})"
                });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Family Member Search Error");
                return BadRequest();
            }
        }

        #region Private Methods

        private static string GetActionsHtml(FamilyMember familyMember, ClaimsPrincipal user)
        {
            string editFamilyMember = string.Empty;
            string deleteFamilyMember = string.Empty;

            if (!user.IsInRole(Roles.SocialAssistance))
            {
                editFamilyMember = $"<a href='/FamilyMember/EditFamilyMember/{familyMember.FamilyMemberId}' data-toggle='modal' " +
                                   "data-target='#modal-action' data-title='Editar Membro Familiar' class='dropdown-item editFamilyMemberButton'><span class='fas fa-edit'></span> Editar </a>";

                deleteFamilyMember = $"<a href='javascript:void(0);' data-url='/FamilyMember/DeleteFamilyMember' data-id='{familyMember.FamilyMemberId}' class='dropdown-item deleteFamilyMemberButton'>" +
                    "<span class='fas fa-trash-alt'></span> Excluir </a>";
            }

            string actionsHtml =
                "<div class='dropdown'>" +
                "  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                "  <div class='dropdown-menu'>" +
                $"      {editFamilyMember}" +
                $"      {deleteFamilyMember}" +
                "  </div>" +
                "</div>";

            return actionsHtml;
        }

        #endregion

    }
}
