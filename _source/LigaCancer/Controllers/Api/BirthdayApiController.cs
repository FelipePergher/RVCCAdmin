// <copyright file="BirthdayApiController.cs" company="Doffs">
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
    [ApiController]
    public class BirthdayApiController : Controller
    {
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<BirthdayApiController> _logger;

        public BirthdayApiController(IDataRepository<Patient> patientService, ILogger<BirthdayApiController> logger)
        {
            _patientService = patientService;
            _logger = logger;
        }

        [Authorize(Roles = Roles.AdminUserSocialAssistanceAuthorize)]
        [HttpPost("~/api/birthday/search")]
        public async Task<IActionResult> BirthdaySearch([FromForm] SearchModel searchModel, [FromForm] BirthdaySearchModel birthdaySearchModel)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<Patient> patients = await ((PatientRepository)_patientService).GetByBirthdayMonth(null, sortColumn, sortDirection, birthdaySearchModel);

                IEnumerable<BirthdayViewModel> data = patients.Select(x => new BirthdayViewModel
                {
                    Name = $"<a href='{Url.Action("Details", "Patient", new { id = x.PatientId })}' target='_blank'>{x.FirstName} {x.Surname}</a>",
                    DateOfBirth = x.DateOfBirth.ToString("dd MMMM"),
                    Phone = x.Phones.FirstOrDefault()?.Number,
                });

                int recordsTotal = _patientService.Count();
                int recordsFiltered = patients.Count();

                return Ok(new { searchModel.Draw, data = data.Skip(skip).Take(take), recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Birthday Search Error");
                return BadRequest();
            }
        }
    }
}
