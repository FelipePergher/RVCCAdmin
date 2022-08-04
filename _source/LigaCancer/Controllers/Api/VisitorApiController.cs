// <copyright file="VisitorApiController.cs" company="Doffs">
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
    public class VisitorApiController : Controller
    {
        private readonly IDataRepository<Visitor> _visitorService;
        private readonly ILogger<VisitorApiController> _logger;

        public VisitorApiController(IDataRepository<Visitor> visitorService, ILogger<VisitorApiController> logger)
        {
            _visitorService = visitorService;
            _logger = logger;
        }

        [HttpPost("~/api/visitor/search")]
        public async Task<IActionResult> VisitorSearch([FromForm] SearchModel searchModel, [FromForm] VisitorSearchModel visitorSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<Visitor> visitors = await _visitorService.GetAllAsync(null, sortColumn, sortDirection, visitorSearch);
                IEnumerable<VisitorViewModel> data = visitors.Select(x => new VisitorViewModel
                {
                    Name = x.Name,
                    CPF = x.CPF,
                    Phone = x.Phone,
                    Actions = GetActionsHtml(x, User)
                }).Skip(skip).Take(take);

                int recordsTotal = _visitorService.Count();
                int recordsFiltered = _visitorService.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Visitor Search Error");
                return BadRequest();
            }
        }
        #region Private Methods

        private static string GetActionsHtml(Visitor visitor, ClaimsPrincipal user)
        {
            string options = $"<a href='/Visitor/EditVisitor/{visitor.VisitorId}' data-toggle='modal' data-target='#modal-action' " +
                             "data-title='Editar Médico' class='dropdown-item editVisitorButton'><span class='fas fa-edit'></span> Editar </a>";

            if (user.IsInRole(Roles.Admin) || user.IsInRole(Roles.Secretary))
            {
                options += $"<a href='javascript:void(0);' data-url='/Visitor/DeleteVisitor' data-id='{visitor.VisitorId}' class='dropdown-item deleteVisitorButton'>" +
                            "<span class='fas fa-trash-alt'></span> Excluir </a>";
            }

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
