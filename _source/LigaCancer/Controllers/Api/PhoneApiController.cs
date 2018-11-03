using System;
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
    [Authorize(Roles = "Admin"), Route("api/[action]")]
    public class PhoneApiController : Controller
    {
        private readonly IDataStore<Phone> _phoneService;

        public PhoneApiController(IDataStore<Phone> phoneService)
        {
            _phoneService = phoneService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPhoneDataTableResponseAsync()
        {
            try
            {
                return Ok(new { data = await _phoneService.GetAllAsync()});
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
