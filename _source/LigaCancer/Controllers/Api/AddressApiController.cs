// <copyright file="AddressApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [ApiController]
    public class AddressApiController : Controller
    {
        private readonly IDataRepository<Address> _addressService;
        private readonly ILogger<AddressApiController> _logger;

        public AddressApiController(IDataRepository<Address> addressService, ILogger<AddressApiController> logger)
        {
            _addressService = addressService;
            _logger = logger;
        }

        [Authorize(Roles = Roles.AdminUserSocialAssistanceAuthorize)]
        [HttpPost("~/api/address/search")]
        public async Task<IActionResult> AddressSearch([FromForm] SearchModel searchModel, [FromForm] AddressSearchModel addressSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                List<Address> addresses = await _addressService.GetAllAsync(null, sortColumn, sortDirection, addressSearch);
                IEnumerable<AddressViewModel> data = addresses.Select(x => new AddressViewModel
                {
                    Street = x.Street,
                    Neighborhood = x.Neighborhood,
                    City = x.City,
                    HouseNumber = x.HouseNumber != null ? x.HouseNumber.ToString() : string.Empty,
                    Complement = x.Complement,
                    ResidenceType = Enums.GetDisplayName(x.ResidenceType),
                    MonthlyAmmountResidence = x.MonthlyAmountResidence == 0 ? string.Empty : x.MonthlyAmountResidence.ToString("C2"),
                    ObservationAddress = x.ObservationAddress,
                    Actions = GetActionsHtml(x, User)
                }).Skip(skip).Take(take);

                int recordsTotal = addresses.Count;

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered = recordsTotal });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Address Search Error");
                return BadRequest();
            }
        }

        #region Private Methods

        private static string GetActionsHtml(Address address, ClaimsPrincipal user)
        {
            string editAddress = string.Empty;
            string deleteAddress = string.Empty;

            if (!user.IsInRole(Roles.SocialAssistance))
            {
                editAddress = $"<a href='/Address/EditAddress/{address.AddressId}' data-toggle='modal' " +
                    "data-target='#modal-action' data-title='Editar Endereço' class='dropdown-item editAddressButton'><span class='fas fa-edit'></span> Editar </a>";

                deleteAddress = $"<a href='javascript:void(0);' data-url='/Address/DeleteAddress' data-id='{address.AddressId}' class='dropdown-item deleteAddressButton'>" +
                    "<span class='fas fa-trash-alt'></span> Excluir </a>";
            }

            string actionsHtml =
                "<div class='dropdown'>" +
                "  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                "  <div class='dropdown-menu'>" +
                $"      {editAddress}" +
                $"      {deleteAddress}" +
                "  </div>" +
                "</div>";

            return actionsHtml;
        }

        #endregion
    }
}
