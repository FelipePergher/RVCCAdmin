using System.Threading.Tasks;
using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Code.Requests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Models.SearchViewModels;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin"), Route("api/[action]")]
    public class PatientApiController : Controller
    {
        private readonly IDataTable<Patient> _patientDataTable;

        public PatientApiController(IDataTable<Patient> patientDataTable)
        {
            _patientDataTable = patientDataTable;
        }

        [HttpPost]
        public async Task<IActionResult> GetPatientDataTableResponseAsync(DataTableOptions options, PatientSearchViewModel patientSearchViewModel)
        {
            try
            {
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>();
                if(int.Parse(patientSearchViewModel.CivilState) != -1) { 
                    Globals.CivilState civilStateValue = (Globals.CivilState) int.Parse(patientSearchViewModel.CivilState);
                    specification.Wheres.Add(x => x.CivilState == civilStateValue);
                }

                if (int.Parse(patientSearchViewModel.Sex) != -1)
                {
                    Globals.Sex sexValue = (Globals.Sex)int.Parse(patientSearchViewModel.Sex);
                    specification.Wheres.Add(x => x.Sex == sexValue);
                }

                return Ok(await _patientDataTable.GetOptionResponseWithSpec(options, specification));
            }
            catch
            {
                return BadRequest();
            }
        }
    }
}
