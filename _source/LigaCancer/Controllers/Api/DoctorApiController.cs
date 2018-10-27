﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Requests;
using LigaCancer.Data.Store;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    public class DoctorApiController : Controller
    {
        private readonly IDataStore<Doctor> _doctorService;

        public DoctorApiController(IDataStore<Doctor> doctorService)
        {
            _doctorService = doctorService;
        }

        public async Task<IActionResult> GetDTResponseAsync(DataTableOptions options)
        {
            try
            {
                BaseSpecification<Doctor> specs = new BaseSpecification<Doctor>(x => x.PatientInformationDoctors);
                return Ok(await ((DoctorStore)_doctorService).GetOptionResponseWithSpec(options, specs));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
