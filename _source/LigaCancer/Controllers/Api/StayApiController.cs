﻿// <copyright file="StayApiController.cs" company="Doffs">
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
    public class StayApiController : Controller
    {
        private readonly IDataRepository<Stay> _stayService;
        private readonly ILogger<StayApiController> _logger;

        public StayApiController(IDataRepository<Stay> stayService, ILogger<StayApiController> logger)
        {
            _stayService = stayService;
            _logger = logger;
        }

        [HttpPost("~/api/stay/search")]
        public async Task<IActionResult> StaySearch([FromForm] SearchModel searchModel, [FromForm] StaySearchModel staySearchModel)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<Stay> stays = await _stayService.GetAllAsync(null, sortColumn, sortDirection, staySearchModel);
                IEnumerable<StayViewModel> data = stays.Select(x => new StayViewModel
                {
                    Patient = x.PatientName,
                    Date = x.StayDateTime.ToDateString(),
                    Note = x.Note,
                    City = x.City,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = string.IsNullOrEmpty(staySearchModel.PatientId)
                    ? _stayService.Count()
                    : ((StayRepository)_stayService).CountByPatient(int.Parse(staySearchModel.PatientId));

                int recordsFiltered = stays.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Stay Search Error");
                return BadRequest();
            }
        }

        #region Private Methods

        private static string GetActionsHtml(Stay stay)
        {
            string options = $"<a href='/Stay/EditStay/{stay.StayId}' data-toggle='modal' data-target='#modal-action' " +
                             "data-title='Editar Presença' class='dropdown-item editStayButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $"<a href='javascript:void(0);' data-url='/Stay/DeleteStay' data-id='{stay.StayId}' " +
                       "class='deleteStayButton dropdown-item'><span class='fas fa-trash-alt'></span> Excluir </a>";

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
