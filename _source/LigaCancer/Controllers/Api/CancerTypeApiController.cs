using System.Threading.Tasks;
using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Code.Requests;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), Route("api/[action]")]
    public class CancerTypeApiController : Controller
    {
        private readonly IDataTable<CancerType> _cancerDataTable;

        public CancerTypeApiController(IDataTable<CancerType> cancerDataTable)
        {
            _cancerDataTable = cancerDataTable;
        }

        [HttpPost]
        public async Task<IActionResult> GetCancerTypeDataTableResponseAsync(DataTableOptions options)
        {
            try
            {
                BaseSpecification<CancerType> specification = new BaseSpecification<CancerType>(x => x.PatientInformationCancerTypes);
                return Ok(await _cancerDataTable.GetOptionResponseWithSpec(options, specification));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
