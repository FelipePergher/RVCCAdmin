// <copyright file="TreatmentTypeApiController.cs" company="Doffs">
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
    public class TreatmentTypeApiController : Controller
    {
        private readonly IDataRepository<TreatmentType> _treatmentTypeService;
        private readonly ILogger<TreatmentTypeApiController> _logger;

        public TreatmentTypeApiController(IDataRepository<TreatmentType> treatmentTypeService, ILogger<TreatmentTypeApiController> logger)
        {
            _treatmentTypeService = treatmentTypeService;
            _logger = logger;
        }

        [HttpPost("~/api/TreatmentType/search")]
        public async Task<IActionResult> TreatmentTypeSearch([FromForm] SearchModel searchModel, [FromForm] TreatmentTypeSearchModel treatmentTypeSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<TreatmentType> treatmentTypes = await _treatmentTypeService.GetAllAsync(new[] { nameof(TreatmentType.PatientTreatmentTypes) }, sortColumn, sortDirection, treatmentTypeSearch);
                IEnumerable<TreatmentTypeViewModel> data = treatmentTypes.Select(x => new TreatmentTypeViewModel
                {
                    Name = x.Name,
                    Note = x.Note,
                    Quantity = x.PatientTreatmentTypes.Count(),
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _treatmentTypeService.Count();
                int recordsFiltered = treatmentTypes.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Treatment Type Search Error");
                return BadRequest();
            }
        }

        [HttpGet("~/api/TreatmentType/select2Get")]
        public async Task<IActionResult> Select2GetTreatmentTypes(string term)
        {
            IEnumerable<TreatmentType> treatmentTypes = await _treatmentTypeService.GetAllAsync(null, "FirstName", "asc", new TreatmentTypeSearchModel { Name = term });
            var select2PagedResult = new Select2PagedResult
            {
                Results = treatmentTypes.Select(x => new Select2Result
                {
                    Id = x.TreatmentTypeId.ToString(),
                    Text = x.Name
                }).ToList(),
            };

            return Ok(select2PagedResult);
        }

        [HttpGet("~/api/TreatmentType/IsNameExist")]
        public async Task<IActionResult> IsNameExist(string name, int treatmentTypeId)
        {
            TreatmentType treatmentType = await ((TreatmentTypeRepository)_treatmentTypeService).FindByNameAsync(name, treatmentTypeId);
            return Ok(treatmentType == null);
        }

        #region Private Methods

        private static string GetActionsHtml(TreatmentType treatmentType)
        {
            string options = $"<a href='/TreatmentType/EditTreatmentType/{treatmentType.TreatmentTypeId}' data-toggle='modal' data-target='#modal-action' " +
                "data-title='Editar Tipo de Tratamento' class='dropdown-item editTreatmentTypeButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $"<a href='javascript:void(0);' data-url='/TreatmentType/DeleteTreatmentType' data-id='{treatmentType.TreatmentTypeId}' " +
                       $"data-relation='{treatmentType.PatientTreatmentTypes.Count > 0}' class='dropdown-item deleteTreatmentTypeButton'>" +
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
