using System.Threading.Tasks;
using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Code.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), Route("api/[action]")]
    public class MedicineApiController : Controller
    {
        private readonly IDataTable<Medicine> _medicineDataTable;

        public MedicineApiController(IDataTable<Medicine> medicineDataTable)
        {
            _medicineDataTable = medicineDataTable;
        }

        [HttpPost]
        public async Task<IActionResult> GetMedicineDataTableResponseAsync(DataTableOptions options)
        {
            try
            {
                BaseSpecification<Medicine> specification = new BaseSpecification<Medicine>(x => x.PatientInformationMedicines);
                return Ok(await _medicineDataTable.GetOptionResponseWithSpec(options, specification));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
