// <copyright file="ServiceTypeApiController.cs" company="Doffs">
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
    public class ServiceTypeApiController : Controller
    {
        private readonly IDataRepository<ServiceType> _serviceTypeService;
        private readonly ILogger<ServiceTypeApiController> _logger;

        public ServiceTypeApiController(IDataRepository<ServiceType> serviceTypeService, ILogger<ServiceTypeApiController> logger)
        {
            _serviceTypeService = serviceTypeService;
            _logger = logger;
        }

        [HttpPost("~/api/ServiceType/search")]
        public async Task<IActionResult> ServiceTypeSearch([FromForm] SearchModel searchModel, [FromForm] ServiceTypeSearchModel serviceTypeSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<ServiceType> serviceTypes = await _serviceTypeService.GetAllAsync(new[] { nameof(ServiceType.PatientInformationServiceTypes) }, sortColumn, sortDirection, serviceTypeSearch);
                IEnumerable<ServiceTypeViewModel> data = serviceTypes.Select(x => new ServiceTypeViewModel
                {
                    Name = x.Name,
                    Quantity = x.PatientInformationServiceTypes.Count(),
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _serviceTypeService.Count();
                int recordsFiltered = serviceTypes.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Service Type Search Error");
                return BadRequest();
            }
        }

        [HttpGet("~/api/ServiceType/select2Get")]
        public async Task<IActionResult> Select2GetServiceTypes(string term)
        {
            IEnumerable<ServiceType> serviceTypes = await _serviceTypeService.GetAllAsync(null, "FirstName", "asc", new ServiceTypeSearchModel { Name = term });
            var select2PagedResult = new Select2PagedResult
            {
                Results = serviceTypes.Select(x => new Select2Result
                {
                    Id = x.ServiceTypeId.ToString(),
                    Text = x.Name
                }).ToList(),
            };

            return Ok(select2PagedResult);
        }

        [HttpGet("~/api/ServiceType/IsNameExist")]
        public async Task<IActionResult> IsNameExist(string name, int serviceTypeId)
        {
            ServiceType serviceType = await ((ServiceTypeRepository)_serviceTypeService).FindByNameAsync(name, serviceTypeId);
            return Ok(serviceType == null);
        }

        #region Private Methods

        private static string GetActionsHtml(ServiceType serviceType)
        {
            string options = $"<a href='/ServiceType/EditServiceType/{serviceType.ServiceTypeId}' data-toggle='modal' data-target='#modal-action' " +
                "data-title='Editar Tipo de Serviço' class='dropdown-item editServiceTypeButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $"<a href='javascript:void(0);' data-url='/ServiceType/DeleteServiceType' data-id='{serviceType.ServiceTypeId}' " +
                       $"data-relation='{serviceType.PatientInformationServiceTypes.Count > 0}' class='dropdown-item deleteServiceTypeButton'>" +
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
