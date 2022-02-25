// <copyright file="CancerTypeApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

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
    [Authorize(Roles = Roles.AdminUserAuthorize)]
    [ApiController]
    public class CancerTypeApiController : Controller
    {
        private readonly IDataRepository<CancerType> _cancerTypeService;
        private readonly ILogger<CancerTypeApiController> _logger;

        public CancerTypeApiController(IDataRepository<CancerType> cancerTypeService, ILogger<CancerTypeApiController> logger)
        {
            _cancerTypeService = cancerTypeService;
            _logger = logger;
        }

        [HttpPost("~/api/CancerType/search")]
        public async Task<IActionResult> CancerTypeSearch([FromForm] SearchModel searchModel, [FromForm] CancerTypeSearchModel cancerTypeSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<CancerType> cancerTypes = await _cancerTypeService.GetAllAsync(new string[] { "PatientInformationCancerTypes" }, sortColumn, sortDirection, cancerTypeSearch);
                IEnumerable<CancerTypeViewModel> data = cancerTypes.Select(x => new CancerTypeViewModel
                {
                    Name = x.Name,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _cancerTypeService.Count();
                int recordsFiltered = cancerTypes.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Cancer Type Search Error", null);
                return BadRequest();
            }
        }

        [HttpGet("~/api/CancerType/select2Get")]
        public async Task<IActionResult> Select2GetCancerTypes(string term)
        {
            IEnumerable<CancerType> cancerTypes = await _cancerTypeService.GetAllAsync(null, "FirstName", "asc", new CancerTypeSearchModel { Name = term });
            var select2PagedResult = new Select2PagedResult
            {
                Results = cancerTypes.Select(x => new Result
                {
                    Id = x.CancerTypeId.ToString(),
                    Text = x.Name
                }).ToList(),
            };

            return Ok(select2PagedResult);
        }

        [HttpGet("~/api/CancerType/IsNameExist")]
        public async Task<IActionResult> IsNameExist(string name, int cancerTypeId)
        {
            CancerType cancerType = await ((CancerTypeRepository)_cancerTypeService).FindByNameAsync(name, cancerTypeId);
            return Ok(cancerType == null);
        }

        #region Private Methods

        private string GetActionsHtml(CancerType cancerType)
        {
            string editCancerType = $"<a href='/CancerType/EditCancerType/{cancerType.CancerTypeId}' data-toggle='modal' data-target='#modal-action' " +
                $"data-title='Editar Tipo de Câncer' class='dropdown-item editCancerTypeButton'><span class='fas fa-edit'></span> Editar </a>";

            string deleteCancerType = $"<a href='javascript:void(0);' data-url='/CancerType/DeleteCancerType' data-id='{cancerType.CancerTypeId}' " +
                $"data-relation='{cancerType.PatientInformationCancerTypes.Count > 0}' class='dropdown-item deleteCancerTypeButton'>" +
                $"<span class='fas fa-trash-alt'></span> Excluir </a>";

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {editCancerType}" +
                $"      {deleteCancerType}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        #endregion
    }
}
