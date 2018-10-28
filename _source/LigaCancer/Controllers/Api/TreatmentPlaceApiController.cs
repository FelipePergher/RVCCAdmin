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
    public class TreatmentPlaceApiController : Controller
    {
        private readonly IDataTable<TreatmentPlace> _treatmentDataTable;

        public TreatmentPlaceApiController(IDataTable<TreatmentPlace> treatmentDataTable)
        {
            _treatmentDataTable = treatmentDataTable;
        }

        public async Task<IActionResult> GetDTResponseAsync(DataTableOptions options)
        {
            try
            {
                BaseSpecification<TreatmentPlace> specs = new BaseSpecification<TreatmentPlace>(x => x.PatientInformationTreatmentPlaces);
                return Ok(await _treatmentDataTable.GetOptionResponseWithSpec(options, specs));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
