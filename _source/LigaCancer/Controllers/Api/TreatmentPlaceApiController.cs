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

        public TreatmentPlaceApiController(IDataStore<TreatmentPlace> treatmentPlaceService)
        {
            _treatmentPlaceService = treatmentPlaceService;
        }

        public async Task<IActionResult> GetDTResponseAsync(DataTableOptions options)
        {
            try
            {
                BaseSpecification<TreatmentPlace> specs = new BaseSpecification<TreatmentPlace>(x => x.PatientInformationTreatmentPlaces);
                return Ok(await ((TreatmentPlaceStore)_treatmentPlaceService).GetOptionResponseWithSpec(options, specs));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
