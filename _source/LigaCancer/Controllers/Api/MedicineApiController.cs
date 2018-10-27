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
    public class MedicineApiController : Controller
    {
        private readonly IDataStore<Medicine> _medicineService;

        public MedicineApiController(IDataStore<Medicine> medicineService)
        {
            _medicineService = medicineService;
        }

        public async Task<IActionResult> GetDTResponseAsync(DataTableOptions options)
        {
            try
            {
                BaseSpecification<Medicine> specs = new BaseSpecification<Medicine>(x => x.PatientInformationMedicines);
                return Ok(await ((MedicineStore)_medicineService).GetOptionResponseWithSpec(options, specs));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
