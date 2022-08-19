// <copyright file="BenefitApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Data.Repositories;
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
    public class BenefitApiController : Controller
    {
        private readonly IDataRepository<Benefit> _benefitService;
        private readonly ILogger<BenefitApiController> _logger;

        public BenefitApiController(IDataRepository<Benefit> benefitService, ILogger<BenefitApiController> logger)
        {
            _benefitService = benefitService;
            _logger = logger;
        }

        [HttpPost("~/api/Benefit/search")]
        public async Task<IActionResult> BenefitSearch([FromForm] SearchModel searchModel, [FromForm] BenefitSearchModel benefitSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;
                IEnumerable<Benefit> benefits = await _benefitService.GetAllAsync(new[] { nameof(Benefit.PatientBenefits) }, sortColumn, sortDirection, benefitSearch);
                IEnumerable<BenefitViewModel> data = benefits.Select(x => new BenefitViewModel
                {
                    Name = x.Name,
                    Note = x.Note,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _benefitService.Count();
                int recordsFiltered = benefits.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Benefit Search Error");
                return BadRequest();
            }
        }

        [HttpGet("~/api/Benefit/select2Get")]
        public async Task<IActionResult> Select2GetBenefits(string term)
        {
            IEnumerable<Benefit> benefits = await _benefitService.GetAllAsync(null, "Name", "asc", new BenefitSearchModel { Name = term });
            var select2PagedResult = new Select2PagedResult
            {
                Results = benefits.Select(x => new Select2Result
                {
                    Id = x.BenefitId.ToString(),
                    Text = x.Name
                }).ToList(),
            };

            return Ok(select2PagedResult);
        }

        [HttpGet("~/api/Benefit/IsNameExist")]
        public async Task<IActionResult> IsNameExist(string name, int benefitId)
        {
            Benefit benefit = await ((BenefitRepository)_benefitService).FindByNameAsync(name, benefitId);

            return Ok(benefit == null);
        }

        #region Private Methods

        private static string GetActionsHtml(Benefit benefit)
        {
            string options = $"<a href='/Benefit/EditBenefit/{benefit.BenefitId}' data-toggle='modal' " +
                             "data-target='#modal-action' data-title='Editar Remédio' class='dropdown-item editBenefitButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $"<a href='javascript:void(0);' data-url='/Benefit/DeleteBenefit' data-id='{benefit.BenefitId}' " +
                       $"data-relation='{benefit.PatientBenefits.Count > 0}' class='dropdown-item deleteBenefitButton'>" +
                       "<span class='fas fa-trash-alt'></span> Excluir </a>";

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
