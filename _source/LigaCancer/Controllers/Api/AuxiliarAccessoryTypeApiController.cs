// <copyright file="AuxiliarAccessoryTypeApiController.cs" company="Doffs">
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
using System.Security.Claims;
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [ApiController]
    public class AuxiliarAccessoryTypeApiController : Controller
    {
        private readonly IDataRepository<AuxiliarAccessoryType> _auxiliarAccessoryTypeService;
        private readonly ILogger<AuxiliarAccessoryTypeApiController> _logger;

        public AuxiliarAccessoryTypeApiController(IDataRepository<AuxiliarAccessoryType> auxiliarAccessoryTypeService, ILogger<AuxiliarAccessoryTypeApiController> logger)
        {
            _auxiliarAccessoryTypeService = auxiliarAccessoryTypeService;
            _logger = logger;
        }

        [HttpPost("~/api/AuxiliarAccessoryType/search")]
        public async Task<IActionResult> AuxiliarAccessoryTypeSearch([FromForm] SearchModel searchModel, [FromForm] AuxiliarAccessoryTypeSearchModel auxiliarAccessoryTypeSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<AuxiliarAccessoryType> auxiliarAccessoryTypes = await _auxiliarAccessoryTypeService.GetAllAsync(new[] { nameof(AuxiliarAccessoryType.PatientAuxiliarAccessoryTypes) }, sortColumn, sortDirection, auxiliarAccessoryTypeSearch);
                IEnumerable<AuxiliarAccessoryTypeViewModel> data = auxiliarAccessoryTypes.Select(x => new AuxiliarAccessoryTypeViewModel
                {
                    Name = x.Name,
                    Quantity = x.PatientAuxiliarAccessoryTypes.Count(),
                    Actions = GetActionsHtml(x, User)
                }).Skip(skip).Take(take);

                int recordsTotal = _auxiliarAccessoryTypeService.Count();
                int recordsFiltered = auxiliarAccessoryTypes.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Expense Type Search Error");
                return BadRequest();
            }
        }

        [HttpGet("~/api/AuxiliarAccessoryType/select2Get")]
        public async Task<IActionResult> Select2GetAuxiliarAccessoryTypes(string term)
        {
            IEnumerable<AuxiliarAccessoryType> auxiliarAccessoryTypes = await _auxiliarAccessoryTypeService.GetAllAsync(null, "FirstName", "asc", new AuxiliarAccessoryTypeSearchModel { Name = term });
            var select2PagedResult = new Select2PagedResult
            {
                Results = auxiliarAccessoryTypes.Select(x => new Select2Result
                {
                    Id = x.AuxiliarAccessoryTypeId.ToString(),
                    Text = x.Name
                }).ToList(),
            };

            return Ok(select2PagedResult);
        }

        [HttpGet("~/api/AuxiliarAccessoryType/IsNameExist")]
        public async Task<IActionResult> IsNameExist(string name, int auxiliarAccessoryTypeId)
        {
            AuxiliarAccessoryType auxiliarAccessoryType = await ((AuxiliarAccessoryTypeRepository)_auxiliarAccessoryTypeService).FindByNameAsync(name, auxiliarAccessoryTypeId);
            return Ok(auxiliarAccessoryType == null);
        }

        #region Private Methods

        private static string GetActionsHtml(AuxiliarAccessoryType auxiliarAccessoryType, ClaimsPrincipal user)
        {
            string options = $"<a href='/AuxiliarAccessoryType/EditAuxiliarAccessoryType/{auxiliarAccessoryType.AuxiliarAccessoryTypeId}' data-toggle='modal' data-target='#modal-action' " +
                "data-title='Editar Tipo de Acessório Auxiliar' class='dropdown-item editAuxiliarAccessoryTypeButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $"<a href='javascript:void(0);' data-url='/AuxiliarAccessoryType/DeleteAuxiliarAccessoryType' data-id='{auxiliarAccessoryType.AuxiliarAccessoryTypeId}' " +
                       $"data-relation='{auxiliarAccessoryType.PatientAuxiliarAccessoryTypes.Count > 0}' class='dropdown-item deleteAuxiliarAccessoryTypeButton'>" +
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
