﻿// <copyright file="FamilyMemberApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
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
                    DateOfBirth = x.DateOfBirth.HasValue ? x.DateOfBirth.Value.ToDateString() : string.Empty,
                    Sex = Enums.GetDisplayName(x.Sex),
                    MonthlyIncome = x.MonthlyIncome.ToString("N2"),
                    IgnoreOnIncome = x.IgnoreOnIncome ? "<span class='fa fa-check'></span>" : string.Empty,
                    Responsible = x.Responsible ? "<span class='fa fa-check'></span>" : string.Empty,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                Patient patient = await _patientService.FindByIdAsync(familyMemberSearch.PatientId);
                int recordsTotal = familyMembers.Count;

                // Remove ignored on calculations
                familyMembers = familyMembers.Where(x => !x.IgnoreOnIncome).ToList();
                var familyIncomeDouble = familyMembers.Sum(x => x.MonthlyIncome) + patient.MonthlyIncome;
                string familyIncome = familyIncomeDouble.ToString("C2");
                string perCapitaIncome = (familyIncomeDouble / (familyMembers.Count + 1)).ToString("C2");

                return Ok(new
                {
                    searchModel.Draw,
                    data,
                    recordsTotal,
                    recordsFiltered = recordsTotal,
                    perCapitaIncome,
                    familyIncome,
                    MonthlyIncome = patient.MonthlyIncome.ToString("N2")
                });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Family Member Search Error");
                return BadRequest();
            }
        }

        #region Private Methods

        private static string GetActionsHtml(FamilyMember familyMember)
        {
            string options = $"<a href='/FamilyMember/EditFamilyMember/{familyMember.FamilyMemberId}' data-toggle='modal' " +
                             "data-target='#modal-action' data-title='Editar Membro Familiar' class='dropdown-item editFamilyMemberButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $"<a href='javascript:void(0);' data-url='/FamilyMember/DeleteFamilyMember' data-id='{familyMember.FamilyMemberId}' class='dropdown-item deleteFamilyMemberButton'>" +
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
