﻿using System.Threading.Tasks;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Models.SearchModel;
using System.Collections.Generic;
using System.Linq;
using LigaCancer.Models.ViewModel;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), ApiController]
    public class TreatmentPlaceApiController : Controller
    {
        private readonly IDataStore<TreatmentPlace> _treatmentPlaceService;

        public TreatmentPlaceApiController(IDataStore<TreatmentPlace> treatmentPlaceService)
        {
            _treatmentPlaceService = treatmentPlaceService;
        }

        [HttpPost("~/api/TreatmentPlace/search")]
        public async Task<IActionResult> TreatmentPlaceSearch([FromForm] SearchModel searchModel, [FromForm] TreatmentPlaceSearchModel City)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<TreatmentPlace> presences = await _treatmentPlaceService.GetAllAsync(sortColumn: sortColumn, sortDirection: sortDirection, filter: City);
                IEnumerable<TreatmentPlaceViewModel> data = presences.Select(x => new TreatmentPlaceViewModel
                {
                    City = x.City,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _treatmentPlaceService.Count();
                int recordsFiltered = presences.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch
            {
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(TreatmentPlace treatmentPlace)
        {
            string editTreatmentPlace = $"<a href='/TreatmentPlace/EditTreatmentPlace/{treatmentPlace.TreatmentPlaceId}' data-toggle='modal' data-target='#modal-action' class='dropdown-item editTreatmentPlaceButton'><i class='fas fa-edit'></i> Editar </a>";

            string deleteTreatmentPlace = $"<a href='javascript:void(0);' data-url='/TreatmentPlace/DeleteTreatmentPlace' data-id='{treatmentPlace.TreatmentPlaceId}' class='dropdown-item deleteTreatmentPlaceButton'><i class='fas fa-trash-alt'></i> Excluir </a>";

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
