using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.SearchModel;
using LigaCancer.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class FamilyMemberApiController : Controller 
    {
        public readonly IDataStore<FamilyMember> _familyMemberService;
        public readonly IDataStore<Patient> _patientService;

        public FamilyMemberApiController(IDataStore<FamilyMember> familyMemberService, IDataStore<Patient> patientService)
        {
            _familyMemberService = familyMemberService;
            _patientService = patientService;
        }

        [HttpPost("~/api/familyMember/search")]
        public async Task<IActionResult> FamilyMemberSearch([FromForm] SearchModel searchModel, [FromForm] FamilyMemberSearchModel familyMemberSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                List<FamilyMember> familyMembers = await _familyMemberService.GetAllAsync(null, sortColumn, sortDirection, familyMemberSearch);
                IEnumerable<FamilyMemberViewModel> data = familyMembers.Select(x => new FamilyMemberViewModel
                {
                    Name = x.Name,
                    Kinship = x.Kinship,
                    Age = x.Age.ToString(),
                    Sex = x.Sex.ToString(),
                    MonthlyIncome = x.MonthlyIncome.ToString("C2"),
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                double monthlyIncomeValue = familyMembers.Sum(x => (double)x.MonthlyIncome);
                string familyIncome = monthlyIncomeValue.ToString("C2");
                string perCapitaIncome= (monthlyIncomeValue / familyMembers.Count).ToString("C2");
                Patient patient = await _patientService.FindByIdAsync(familyMemberSearch.PatientId);
                
                int recordsTotal = familyMembers.Count;

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered = recordsTotal, perCapitaIncome, familyIncome, monthlyIncome = patient.MonthlyIncome.ToString("C2") });
            }
            catch
            {
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(FamilyMember familyMember)
        {
            string editFamilyMember = $"<a href='/FamilyMember/EditFamilyMember/{familyMember.FamilyMemberId}' data-toggle='modal' " +
                $"data-target='#modal-action-secondary' data-title='Editar Membro Familiar' class='dropdown-item editFamilyMemberButton'><i class='fas fa-edit'></i> Editar </a>";

            string deleteFamilyMember = $"<a href='javascript:void(0);' data-url='/FamilyMember/DeleteFamilyMember' data-id='{familyMember.FamilyMemberId}' class='dropdown-item deleteFamilyMemberButton'>" +
                $"<i class='fas fa-trash-alt'></i> Excluir </a>";

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {editFamilyMember}" +
                $"      {deleteFamilyMember}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        #endregion

    }
}
