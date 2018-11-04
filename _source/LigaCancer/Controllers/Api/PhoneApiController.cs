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
    [Authorize(Roles = "Admin"), Route("api/[action]/{id?}")]
    public class PhoneApiController : Controller
    {
        private readonly IDataStore<Patient> _patientService;

        public PhoneApiController(IDataStore<Patient> patientService)
        {
            _patientService = patientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPhoneDataTableResponseAsync(string id)
        {
            try
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.Phones);
                Patient patient = await _patientService.FindByIdAsync(id, specification);
                return Ok(new { data = patient.Phones});
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
