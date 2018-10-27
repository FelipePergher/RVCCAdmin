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
    [Authorize(Roles = "Admin")]
    public class CancerTypeApiController : Controller
    {
        private readonly IDataStore<CancerType> _cancerTypeService;

        public CancerTypeApiController(IDataStore<CancerType> cancerTypeService)
        {
            _cancerTypeService = cancerTypeService;
        }

        public async Task<IActionResult> GetDTResponseAsync(DataTableOptions options)
        {
            try
            {
                BaseSpecification<CancerType> specs = new BaseSpecification<CancerType>(x => x.PatientInformationCancerTypes);
                return Ok(await ((CancerTypeStore)_cancerTypeService).GetOptionResponseWithSpec(options, specs));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
