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
        private readonly IDataTable<Doctor> _doctorDataTable;

        public DoctorApiController(IDataTable<Doctor> doctorDataTable)
        {
            _doctorDataTable = doctorDataTable;
        }

        public async Task<IActionResult> GetDTResponseAsync(DataTableOptions options)
        {
            try
            {
                BaseSpecification<Doctor> specs = new BaseSpecification<Doctor>(x => x.PatientInformationDoctors);
                return Ok(await _doctorDataTable.GetOptionResponseWithSpec(options, specs));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
