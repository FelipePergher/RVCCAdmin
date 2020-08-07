﻿// <copyright file="SaleShirt2020Controller.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.FormModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminUserAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class SaleShirt2020Controller : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataRepository<SaleShirt2020> _saleShirt2020Service;
        private readonly ILogger<SaleShirt2020Controller> _logger;

        public SaleShirt2020Controller(
            IDataRepository<SaleShirt2020> saleShirt2020Service,
            ILogger<SaleShirt2020Controller> logger,
            UserManager<ApplicationUser> userManager)
        {
            _saleShirt2020Service = saleShirt2020Service;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult AddSaleShirt2020()
        {
            return PartialView("Partials/_AddSaleShirt2020", new SaleShirt2020FormModel());
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Details(string id)
        {
            SaleShirt2020 saleShirt2020 = await _saleShirt2020Service.FindByIdAsync(id);

            var salesShirtModel = new SaleShirt2020FormModel
            {
                DateOrdered = saleShirt2020.DateOrdered.ToShortDateString(),
                BuyerName = saleShirt2020.BuyerName,
                BuyerPhone = saleShirt2020.BuyerPhone,
                MaskQuantity = saleShirt2020.MaskQuantity,

                // Normal Shirts
                SizePBabyLookQuantity = saleShirt2020.SizePBabyLookQuantity,
                SizeMBabyLookQuantity = saleShirt2020.SizeMBabyLookQuantity,
                SizeGBabyLookQuantity = saleShirt2020.SizeGBabyLookQuantity,
                SizeGGBabyLookQuantity = saleShirt2020.SizeGGBabyLookQuantity,

                // baby look shirts
                Size4NormalQuantity = saleShirt2020.Size4NormalQuantity,
                Size8NormalQuantity = saleShirt2020.Size8NormalQuantity,
                Size12NormalQuantity = saleShirt2020.Size12NormalQuantity,
                Size14NormalQuantity = saleShirt2020.Size14NormalQuantity,
                Size16NormalQuantity = saleShirt2020.Size16NormalQuantity,
                SizePNormalQuantity = saleShirt2020.SizePNormalQuantity,
                SizeMNormalQuantity = saleShirt2020.SizeMNormalQuantity,
                SizeGNormalQuantity = saleShirt2020.SizeGNormalQuantity,
                SizeGGNormalQuantity = saleShirt2020.SizeGGNormalQuantity,

                PriceTotal = saleShirt2020.PriceTotal,
                ShirtQuantityTotal = saleShirt2020.ShirtQuantityTotal,

                // Dates
                DatePayment = saleShirt2020.DatePayment == DateTime.MinValue ? string.Empty : saleShirt2020.DatePayment.ToShortDateString(),
                DateConfection = saleShirt2020.DateConfection == DateTime.MinValue ? string.Empty : saleShirt2020.DateConfection.ToShortDateString(),
                DateProduced = saleShirt2020.DateProduced == DateTime.MinValue ? string.Empty : saleShirt2020.DateProduced.ToShortDateString(),
                DateCollected = saleShirt2020.DateCollected == DateTime.MinValue ? string.Empty : saleShirt2020.DateCollected.ToShortDateString(),
                DateCanceled = saleShirt2020.DateCanceled == DateTime.MinValue ? string.Empty : saleShirt2020.DateCanceled.ToShortDateString(),
            };

            if (User.Identity.IsAuthenticated)
            {
                return PartialView("Partials/_DetailsSaleShirt2020", salesShirtModel);
            }

            return View("PublicDetailsSaleShirt2020", salesShirtModel);
        }

        [HttpPost]
        public async Task<IActionResult> AddSaleShirt2020(SaleShirt2020FormModel saleShirt2020Form)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);
                var dateTime = DateTime.Parse(saleShirt2020Form.DateOrdered);

                int shirtQuantity =
                    saleShirt2020Form.SizePBabyLookQuantity + saleShirt2020Form.SizeMBabyLookQuantity +
                    saleShirt2020Form.SizeGBabyLookQuantity + saleShirt2020Form.SizeGGBabyLookQuantity +
                    saleShirt2020Form.Size4NormalQuantity + saleShirt2020Form.Size8NormalQuantity +
                    saleShirt2020Form.Size12NormalQuantity + saleShirt2020Form.Size14NormalQuantity +
                    saleShirt2020Form.Size16NormalQuantity + saleShirt2020Form.SizePNormalQuantity +
                    saleShirt2020Form.SizeMNormalQuantity + saleShirt2020Form.SizeGNormalQuantity +
                    saleShirt2020Form.SizeGGNormalQuantity;

                var saleShirt2020 = new SaleShirt2020
                {
                    DateOrdered = dateTime,
                    BuyerName = saleShirt2020Form.BuyerName,
                    BuyerPhone = saleShirt2020Form.BuyerPhone,
                    MaskQuantity = saleShirt2020Form.MaskQuantity,

                    // Normal Shirts
                    SizePBabyLookQuantity = saleShirt2020Form.SizePBabyLookQuantity,
                    SizeMBabyLookQuantity = saleShirt2020Form.SizeMBabyLookQuantity,
                    SizeGBabyLookQuantity = saleShirt2020Form.SizeGBabyLookQuantity,
                    SizeGGBabyLookQuantity = saleShirt2020Form.SizeGGBabyLookQuantity,

                    // baby look shirts
                    Size4NormalQuantity = saleShirt2020Form.Size4NormalQuantity,
                    Size8NormalQuantity = saleShirt2020Form.Size8NormalQuantity,
                    Size12NormalQuantity = saleShirt2020Form.Size12NormalQuantity,
                    Size14NormalQuantity = saleShirt2020Form.Size14NormalQuantity,
                    Size16NormalQuantity = saleShirt2020Form.Size16NormalQuantity,
                    SizePNormalQuantity = saleShirt2020Form.SizePNormalQuantity,
                    SizeMNormalQuantity = saleShirt2020Form.SizeMNormalQuantity,
                    SizeGNormalQuantity = saleShirt2020Form.SizeGNormalQuantity,
                    SizeGGNormalQuantity = saleShirt2020Form.SizeGGNormalQuantity,

                    PriceTotal = (shirtQuantity * 20) + (saleShirt2020Form.MaskQuantity * 5),
                    ShirtQuantityTotal = shirtQuantity,
                    CreatedBy = user.Name
                };

                TaskResult result = await _saleShirt2020Service.CreateAsync(saleShirt2020);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_AddSaleShirt2020", saleShirt2020Form);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> UpdateStatusSaleShirt2020(string id, Enums.Status status, string date)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            SaleShirt2020 saleShirt2020 = await _saleShirt2020Service.FindByIdAsync(id);

            if (saleShirt2020 == null)
            {
                return NotFound();
            }

            saleShirt2020.Status = status;

            var dateTime = DateTime.Parse(date);
            switch (status)
            {
                case Enums.Status.Ordered:
                    saleShirt2020.DateConfection = dateTime;
                    break;
                case Enums.Status.PaymentReceived:
                    saleShirt2020.DatePayment = dateTime;
                    break;
                case Enums.Status.SentToProduction:
                    saleShirt2020.DateConfection = dateTime;
                    break;
                case Enums.Status.Produced:
                    saleShirt2020.DateProduced = dateTime;
                    break;
                case Enums.Status.Collected:
                    saleShirt2020.DateCollected = dateTime;
                    break;
                case Enums.Status.Canceled:
                    saleShirt2020.DateCanceled = dateTime;
                    break;
            }

            TaskResult result = await _saleShirt2020Service.UpdateAsync(saleShirt2020);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest(result);
        }
    }
}