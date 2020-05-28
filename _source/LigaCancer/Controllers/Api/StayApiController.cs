﻿// <copyright file="StayApiController.cs" company="Felipe Pergher">
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

        [Authorize(Roles = Roles.AdminUserSocialAssistanceAuthorize)]
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
                    Date = x.StayDateTime.ToString("dd/MM/yyyy"),
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
                _logger.LogError(e, "Stay Search Error", null);
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(Stay stay)
        {
            string editStay = string.Empty;
            string deleteStay = string.Empty;
            if (!User.IsInRole(Roles.SocialAssistance))
            {
                editStay = $"<a href='/Stay/EditStay/{stay.StayId}' data-toggle='modal' data-target='#modal-action' " +
                   $"data-title='Editar Presença' class='dropdown-item editStayButton'><i class='fas fa-edit'></i> Editar </a>";
                deleteStay = $"<a href='javascript:void(0);' data-url='/Stay/DeleteStay' data-id='{stay.StayId}' " +
                    $"class='deleteStayButton dropdown-item'><i class='fas fa-trash-alt'></i> Excluir </a>";

                TimeSpan diff = DateTime.Now.Subtract(stay.RegisterTime);
                if (diff.Days > 0)
                {
                    editStay = string.Empty;
                }
            }

            string actionsHtml =
                $@"<div class='dropdown'>
                  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>
                  <div class='dropdown-menu'>
                      {editStay}
                      {deleteStay}
                  </div>
                </div>";

            return actionsHtml;
        }

        #endregion
    }
}