// <copyright file="TreatmentPlaceApiController.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
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
    public class TreatmentPlaceApiController : Controller
    {
        private readonly IDataRepository<TreatmentPlace> _treatmentPlaceService;
        private readonly ILogger<TreatmentPlaceApiController> _logger;

        public TreatmentPlaceApiController(IDataRepository<TreatmentPlace> treatmentPlaceService, ILogger<TreatmentPlaceApiController> logger)
        {
            _treatmentPlaceService = treatmentPlaceService;
            _logger = logger;
        }

        [HttpPost("~/api/TreatmentPlace/search")]
        public async Task<IActionResult> TreatmentPlaceSearch([FromForm] SearchModel searchModel, [FromForm] TreatmentPlaceSearchModel treatmentPlaceSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<TreatmentPlace> presences = await _treatmentPlaceService.GetAllAsync(new string[] { "PatientInformationTreatmentPlaces" }, sortColumn, sortDirection, treatmentPlaceSearch);
                IEnumerable<TreatmentPlaceViewModel> data = presences.Select(x => new TreatmentPlaceViewModel
                {
                    City = x.City,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _treatmentPlaceService.Count();
                int recordsFiltered = presences.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Treatment Place Search Error", null);
                return BadRequest();
            }
        }

        [HttpGet("~/api/TreatmentPlace/select2Get")]
        public async Task<IActionResult> Select2GetTreatmentPlaces(string term)
        {
            IEnumerable<TreatmentPlace> treatmentPlaces = await _treatmentPlaceService.GetAllAsync(null, "City", "asc", new TreatmentPlaceSearchModel { City = term });
            var select2PagedResult = new Select2PagedResult
            {
                Results = treatmentPlaces.Select(x => new Result
                {
                    Id = x.TreatmentPlaceId.ToString(),
                    Text = x.City
                }).ToList(),
            };

            return Ok(select2PagedResult);
        }

        [HttpGet("~/api/TreatmentPlace/IsCityExist")]
        public async Task<IActionResult> IsCityExist(string city, int treatmentPlaceId)
        {
            TreatmentPlace treatmentPlace = await ((TreatmentPlaceRepository)_treatmentPlaceService).FindByCityAsync(city, treatmentPlaceId);
            return Ok(treatmentPlace == null);
        }

        #region Private Methods

        private string GetActionsHtml(TreatmentPlace treatmentPlace)
        {
            string editTreatmentPlace = $"<a href='/TreatmentPlace/EditTreatmentPlace/{treatmentPlace.TreatmentPlaceId}' data-toggle='modal' " +
                $"data-target='#modal-action' data-title='Editar Cidade' class='dropdown-item editTreatmentPlaceButton'><i class='fas fa-edit'></i> Editar </a>";

            string deleteTreatmentPlace = $"<a href='javascript:void(0);' data-url='/TreatmentPlace/DeleteTreatmentPlace' data-id='{treatmentPlace.TreatmentPlaceId}' " +
                $"data-relation='{treatmentPlace.PatientInformationTreatmentPlaces.Count > 0}' class='dropdown-item deleteTreatmentPlaceButton'>" +
                $"<i class='fas fa-trash-alt'></i> Excluir </a>";

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {editTreatmentPlace}" +
                $"      {deleteTreatmentPlace}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        #endregion
    }
}
