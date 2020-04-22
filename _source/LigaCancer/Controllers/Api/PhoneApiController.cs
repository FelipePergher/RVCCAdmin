using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.PatientModels;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [Authorize(Roles = Roles.AdminAndUserAuthorize)]
    [ApiController]
    public class PhoneApiController : Controller
    {
        private readonly IDataRepository<Phone> _phoneService;
        private readonly ILogger<PhoneApiController> _logger;

        public PhoneApiController(IDataRepository<Phone> phoneService, ILogger<PhoneApiController> logger)
        {
            _phoneService = phoneService;
            _logger = logger;
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
                    PhoneType = Globals.GetDisplayName(x.PhoneType),
                    ObservationNote = x.ObservationNote,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = phones.Count;

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered = recordsTotal });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Phone Search Error", null);
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(Phone phone)
        {
            string editPhone = $"<a href='/Phone/EditPhone/{phone.PhoneId}' data-toggle='modal' " +
                $"data-target='#modal-action' data-title='Editar Telefone' class='dropdown-item editPhoneButton'><i class='fas fa-edit'></i> Editar </a>";

            string deletePhone = $"<a href='javascript:void(0);' data-url='/Phone/DeletePhone' data-id='{phone.PhoneId}' class='dropdown-item deletePhoneButton'>" +
                $"<i class='fas fa-trash-alt'></i>  Excluir </a>";

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
