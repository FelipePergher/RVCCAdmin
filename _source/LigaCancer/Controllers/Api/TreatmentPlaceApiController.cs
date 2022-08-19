// <copyright file="TreatmentPlaceApiController.cs" company="Doffs">
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

                IEnumerable<TreatmentPlace> treatmentPlaces = await _treatmentPlaceService.GetAllAsync(new[] { nameof(TreatmentPlace.PatientInformationTreatmentPlaces) }, sortColumn, sortDirection, treatmentPlaceSearch);
                IEnumerable<TreatmentPlaceViewModel> data = treatmentPlaces.Select(x => new TreatmentPlaceViewModel
                {
                    City = x.City,
                    Quantity = x.PatientInformationTreatmentPlaces.Count(),
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _treatmentPlaceService.Count();
                int recordsFiltered = treatmentPlaces.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Treatment Place Search Error");
                return BadRequest();
            }
        }

        [HttpGet("~/api/TreatmentPlace/select2Get")]
        public async Task<IActionResult> Select2GetTreatmentPlaces(string term)
        {
            IEnumerable<TreatmentPlace> treatmentPlaces = await _treatmentPlaceService.GetAllAsync(null, "City", "asc", new TreatmentPlaceSearchModel { City = term });
            var select2PagedResult = new Select2PagedResult
            {
                Results = treatmentPlaces.Select(x => new Select2Result
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

        private static string GetActionsHtml(TreatmentPlace treatmentPlace)
        {
            string options = $"<a href='/TreatmentPlace/EditTreatmentPlace/{treatmentPlace.TreatmentPlaceId}' data-toggle='modal' " +
                             "data-target='#modal-action' data-title='Editar Cidade' class='dropdown-item editTreatmentPlaceButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $"<a href='javascript:void(0);' data-url='/TreatmentPlace/DeleteTreatmentPlace' data-id='{treatmentPlace.TreatmentPlaceId}' " +
                       $"data-relation='{treatmentPlace.PatientInformationTreatmentPlaces.Count > 0}' class='dropdown-item deleteTreatmentPlaceButton'>" +
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
