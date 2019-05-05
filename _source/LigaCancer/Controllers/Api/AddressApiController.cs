using LigaCancer.Code;
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
    public class AddressApiController : Controller
    {
        private readonly IDataStore<Address> _addressService;

        public AddressApiController(IDataStore<Address> addressService)
        {
            _addressService = addressService;
        }

        [HttpPost("~/api/address/search")]
        public async Task<IActionResult> AddressSearch([FromForm] SearchModel searchModel, [FromForm] AddressSearchModel addressSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                List<Address> addresses = await _addressService.GetAllAsync(null, sortColumn, sortDirection, addressSearch);
                IEnumerable<AddressViewModel> data = addresses.Select(x => new AddressViewModel
                {
                    Street = x.Street,
                    Neighborhood = x.Neighborhood,
                    City = x.City,
                    HouseNumber = x.HouseNumber.ToString(),
                    Complement = x.Complement,
                    ResidenceType = Globals.GetDisplayName(x.ResidenceType),
                    MonthlyAmmountResidence = x.MonthlyAmmountResidence.ToString(),
                    ObservationAddress = x.ObservationAddress,
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = addresses.Count;

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered = recordsTotal });
            }
            catch
            {
                return BadRequest();
            }
        }

        #region Private Methods

        private string GetActionsHtml(Address address)
        {
            string editAddress = $"<a href='/Address/EditAddress/{address.AddressId}' data-toggle='modal' " +
                $"data-target='#modal-action-secondary' data-title='Editar Endereço' class='dropdown-item editAddressButton'><i class='fas fa-edit'></i> Editar </a>";

            string deleteAddress = $"<a href='javascript:void(0);' data-url='/Address/DeleteAddress' data-id='{address.AddressId}' class='dropdown-item deleteAddressButton'>" +
                $"<i class='fas fa-trash-alt'></i> Excluir </a>";

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {editAddress}" +
                $"      {deleteAddress}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        #endregion
    }
}
