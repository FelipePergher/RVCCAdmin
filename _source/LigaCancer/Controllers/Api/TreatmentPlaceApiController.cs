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
    public class TreatmentPlaceApiController : Controller
    {
        private readonly IDataStore<TreatmentPlace> _treatmentPlaceService;
        protected readonly ILogger<HomeController> _logger;

        public TreatmentPlaceApiController(IDataStore<TreatmentPlace> treatmentPlaceService, ILogger<HomeController> logger)
        {
            _treatmentPlaceService = treatmentPlaceService;
            _logger = logger;
        }

        public async Task<IActionResult> GetDTResponseAsync(DataTableOptions options)
        {
            try
            {
                BaseSpecification<TreatmentPlace> specs = new BaseSpecification<TreatmentPlace>(x => x.PatientInformationTreatmentPlaces);
                return Ok(await ((TreatmentPlaceStore)_treatmentPlaceService).GetOptionResponseWithSpec(options, specs));
            }
            catch (Exception e)
            {
                _logger.LogError("Error Occurred While Running GetOptions @ HomeController : \n" + e.Message);
                return BadRequest();
            }
        }
    }
}
