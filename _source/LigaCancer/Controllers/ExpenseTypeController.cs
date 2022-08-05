// <copyright file="ExpenseTypeController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Models.FormModel;
using RVCC.Models.SearchModel;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.AdminSecretarySocialAssistanceAuthorize)]
    [AutoValidateAntiforgeryToken]
    public class ExpenseTypeController : Controller
    {
        private readonly IDataRepository<ExpenseType> _expenseTypeService;
        private readonly ILogger<ExpenseTypeController> _logger;

        public ExpenseTypeController(IDataRepository<ExpenseType> expenseTypeService, ILogger<ExpenseTypeController> logger)
        {
            _expenseTypeService = expenseTypeService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new ExpenseTypeSearchModel());
        }

        [HttpGet]
        public IActionResult AddExpenseType()
        {
            return PartialView("Partials/_AddExpenseType", new ExpenseTypeFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddExpenseType(ExpenseTypeFormModel expenseTypeForm)
        {
            if (ModelState.IsValid)
            {
                var expenseType = new ExpenseType(expenseTypeForm.Name, expenseTypeForm.ExpenseTypeFrequency);

                TaskResult result = await _expenseTypeService.CreateAsync(expenseType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.InsertItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_AddExpenseType", expenseTypeForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditExpenseType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            ExpenseType expenseType = await _expenseTypeService.FindByIdAsync(id);

            if (expenseType == null)
            {
                return NotFound();
            }

            var expenseTypeForm = new ExpenseTypeFormModel(expenseType.Name, expenseType.ExpenseTypeFrequency, expenseType.ExpenseTypeId);

            return PartialView("Partials/_EditExpenseType", expenseTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditExpenseType(string id, ExpenseTypeFormModel expenseTypeForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                ExpenseType expenseType = await _expenseTypeService.FindByIdAsync(id);

                if (expenseType == null)
                {
                    return NotFound();
                }

                expenseType.Name = expenseTypeForm.Name;
                expenseType.ExpenseTypeFrequency = expenseTypeForm.ExpenseTypeFrequency;

                TaskResult result = await _expenseTypeService.UpdateAsync(expenseType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(LogEvents.UpdateItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                return BadRequest();
            }

            return PartialView("Partials/_EditExpenseType", expenseTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteExpenseType([FromForm] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            ExpenseType expenseType = await _expenseTypeService.FindByIdAsync(id);

            if (expenseType == null)
            {
                return NotFound();
            }

            TaskResult result = await _expenseTypeService.DeleteAsync(expenseType);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(LogEvents.DeleteItem, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
            return BadRequest(result);
        }
    }
}
