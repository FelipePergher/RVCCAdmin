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
    public class PhoneApiController : Controller
    {
        private readonly IDataStore<Patient> _patientService;
        private readonly IDataStore<Phone> _phoneService;

        public PhoneApiController(IDataStore<Patient> patientService, IDataStore<Phone> phoneService)
        {
            _patientService = patientService;
            _phoneService = phoneService;
        }

        [HttpPost("~/api/phone/search")]
        public async Task<IActionResult> PhoneSearch([FromForm] SearchModel searchModel, [FromForm] PhoneSearchModel phoneSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;
                
                List<Phone> phones = await _phoneService.GetAllAsync(null, sortColumn, sortDirection, phoneSearch);
                IEnumerable<PhoneViewModel> data = phones.Select(x => new PhoneViewModel
                {
                    Number = x.Number,
                    PhoneType = x.PhoneType.ToString(),
                    ObservationNote = x.ObservationNote,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = phones.Count;

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered = recordsTotal });
            }
            catch
            {
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(Phone phone)
        {
            string editPhone = $"<a href='/Phone/EditPhone/{phone.PhoneId}' data-toggle='modal' " +
                $"data-target='#modal-action-secondary' data-title='Editar Telefone' class='dropdown-item editPhoneButton'><i class='fas fa-edit'></i> Editar </a>";

            string deletePhone = $"<a href='javascript:void(0);' data-url='/Phone/DeletePhone' data-id='{phone.PhoneId}' class='dropdown-item deletePhoneButton'>" +
                $"<i class='fas fa-trash-alt'></i> Excluir </a>";

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {editPhone}" +
                $"      {deletePhone}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        #endregion

    }
}
