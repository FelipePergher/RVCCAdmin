// <copyright file="ExpenseTypeApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Data.Repositories;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [ApiController]
    public class ExpenseTypeApiController : Controller
    {
        private readonly IDataRepository<ExpenseType> _expenseTypeService;
        private readonly ILogger<ExpenseTypeApiController> _logger;

        public ExpenseTypeApiController(IDataRepository<ExpenseType> expenseTypeService, ILogger<ExpenseTypeApiController> logger)
        {
            _expenseTypeService = expenseTypeService;
            _logger = logger;
        }

        [HttpPost("~/api/ExpenseType/search")]
        public async Task<IActionResult> ExpenseTypeSearch([FromForm] SearchModel searchModel, [FromForm] ExpenseTypeSearchModel expenseTypeSearch)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<ExpenseType> expenseTypes = await _expenseTypeService.GetAllAsync(new[] { nameof(ExpenseType.PatientExpenseTypes) }, sortColumn, sortDirection, expenseTypeSearch);
                IEnumerable<ExpenseTypeViewModel> data = expenseTypes.Select(x => new ExpenseTypeViewModel
                {
                    Name = x.Name,
                    ExpenseTypeFrequency = Enums.GetDisplayName(x.ExpenseTypeFrequency),
                    Quantity = x.PatientExpenseTypes.Count(),
                    Actions = GetActionsHtml(x)
                }).Skip(skip).Take(take);

                int recordsTotal = _expenseTypeService.Count();
                int recordsFiltered = expenseTypes.Count();

                return Ok(new { searchModel.Draw, data, recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Expense Type Search Error");
                return BadRequest();
            }
        }

        [HttpGet("~/api/ExpenseType/select2Get")]
        public async Task<IActionResult> Select2GetExpenseTypes(string term)
        {
            IEnumerable<ExpenseType> expenseTypes = await _expenseTypeService.GetAllAsync(null, "FirstName", "asc", new ExpenseTypeSearchModel { Name = term });
            var select2PagedResult = new Select2PagedResult
            {
                Results = expenseTypes.Select(x => new Select2Result
                {
                    Id = x.ExpenseTypeId.ToString(),
                    Text = x.Name
                }).ToList(),
            };

            return Ok(select2PagedResult);
        }

        [HttpGet("~/api/ExpenseType/IsNameExist")]
        public async Task<IActionResult> IsNameExist(string name, int expenseTypeId)
        {
            ExpenseType expenseType = await ((ExpenseTypeRepository)_expenseTypeService).FindByNameAsync(name, expenseTypeId);
            return Ok(expenseType == null);
        }

        #region Private Methods

        private static string GetActionsHtml(ExpenseType expenseType)
        {
            string options = $"<a href='/ExpenseType/EditExpenseType/{expenseType.ExpenseTypeId}' data-toggle='modal' data-target='#modal-action' " +
                "data-title='Editar Tipo de Despesa' class='dropdown-item editExpenseTypeButton'><span class='fas fa-edit'></span> Editar </a>";

            options += $"<a href='javascript:void(0);' data-url='/ExpenseType/DeleteExpenseType' data-id='{expenseType.ExpenseTypeId}' " +
                       $"data-relation='{expenseType.PatientExpenseTypes.Count > 0}' class='dropdown-item deleteExpenseTypeButton'>" +
                       "<span class='fas fa-trash-alt'></span> Excluir </a>";

            string actionsHtml =
                "<div class='dropdown'>" +
                "  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                "  <div class='dropdown-menu'>" +
                $"      {options}" +
                "  </div>" +
                "</div>";

            return actionsHtml;
        }

        #endregion
    }
}
