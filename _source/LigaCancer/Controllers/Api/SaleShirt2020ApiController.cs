﻿// <copyright file="SaleShirt2020ApiController.cs" company="Doffs">
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
    public class SaleShirt2020ApiController : Controller
    {
        private readonly IDataRepository<SaleShirt2020> _saleShirt2020Service;
        private readonly ILogger<SaleShirt2020ApiController> _logger;

        public SaleShirt2020ApiController(IDataRepository<SaleShirt2020> saleShirt2020Service, ILogger<SaleShirt2020ApiController> logger)
        {
            _saleShirt2020Service = saleShirt2020Service;
            _logger = logger;
        }

        [HttpPost("~/api/SaleShirt2020/search")]
        public async Task<IActionResult> SaleShirt2020Search([FromForm] SearchModel searchModel, [FromForm] SaleShirt2020SearchModel saleShirt2020Search)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<SaleShirt2020> saleShirts2020 = await _saleShirt2020Service.GetAllAsync(null, sortColumn, sortDirection, saleShirt2020Search);
                IEnumerable<SaleShirt2020ViewModel> data = saleShirts2020.Select(x => new SaleShirt2020ViewModel
                {
                    Code = $"rvcc{x.ShirtSaleId}",
                    BuyerName = x.BuyerName,
                    BuyerPhone = x.BuyerPhone,
                    Date = x.DateOrdered.ToShortDateString(),
                    MaskQuantity = x.MaskQuantity,
                    PriceTotal = x.PriceTotal,
                    ShirtQuantityTotal = x.ShirtQuantityTotal,
                    Status = Enums.GetDisplayName(x.Status),
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _saleShirt2020Service.Count();
                int recordsFiltered = saleShirts2020.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "SaleShirt2020 Search Error", null);
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(SaleShirt2020 saleShirt2020)
        {
            string nextStatus = string.Empty;
            string nextStatusValue = string.Empty;

            switch (saleShirt2020.Status)
            {
                case Enums.Status.Ordered:
                    nextStatus = Enums.GetDisplayName(Enums.Status.PaymentReceived);
                    nextStatusValue = Enums.Status.PaymentReceived.ToString();
                    break;
                case Enums.Status.PaymentReceived:
                    nextStatus = Enums.GetDisplayName(Enums.Status.SentToProduction);
                    nextStatusValue = Enums.Status.SentToProduction.ToString();
                    break;
                case Enums.Status.SentToProduction:
                    nextStatus = Enums.GetDisplayName(Enums.Status.Produced);
                    nextStatusValue = Enums.Status.Produced.ToString();
                    break;
                case Enums.Status.Produced:
                    nextStatus = Enums.GetDisplayName(Enums.Status.Collected);
                    nextStatusValue = Enums.Status.Collected.ToString();
                    break;
            }

            string updateStatusSaleShirt2020 = string.IsNullOrEmpty(nextStatus)
                ? string.Empty
                : $@"
                <a href='javascript:void(0);' data-url='/SaleShirt2020/UpdateStatusSaleShirt2020' data-id='{saleShirt2020.ShirtSaleId}' data-status='{nextStatusValue}' class='dropdown-item updateStatusSaleShirt2020Button'>
                    <i class='fas fa-hand-point-right'></i> 
                    {nextStatus}
                </a>";

            string cancelSaleShirt2020 = saleShirt2020.Status != Enums.Status.Ordered
                ? string.Empty
                : $@"
                <a href='javascript:void(0);' data-url='/SaleShirt2020/UpdateStatusSaleShirt2020' data-id='{saleShirt2020.ShirtSaleId}' data-status='{Enums.Status.Canceled.ToString()}' class='dropdown-item updateStatusSaleShirt2020Button'>
                    <i class='fas fa-trash-alt'></i> 
                    Cancelar
                </a>";

            string detailsSaleShirt2020 = $"<a href='{Url.Action("Details", "SaleShirt2020", new { id = saleShirt2020.ShirtSaleId })}' data-toggle='modal' " +
                                          $"data-target='#modal-action' data-title='Detalhes' class='dropdown-item detailsSaleShirt2020Button'><i class='fas fa-info'></i> Detalhes </a>";

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {detailsSaleShirt2020}" +
                $"      {updateStatusSaleShirt2020}" +
                $"      {cancelSaleShirt2020}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        #endregion
    }
}