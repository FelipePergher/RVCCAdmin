// <copyright file="AdminController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.FormModel;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    [AutoValidateAntiforgeryToken]
    public class AdminController : Controller
    {
        private readonly IDataRepository<AdminInfo> _adminInfoService;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IDataRepository<AdminInfo> adminInfoService, ILogger<AdminController> logger)
        {
            _adminInfoService = adminInfoService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            AdminInfo adminInfo = (await _adminInfoService.GetAllAsync()).FirstOrDefault();

            var adminForm = new AdminFormModel
            {
                MinSalary = adminInfo.MinSalary.ToString()
            };

            return View(adminForm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AdminFormModel adminForm)
        {
            if (ModelState.IsValid)
            {
                AdminInfo adminInfo = (await _adminInfoService.GetAllAsync()).FirstOrDefault();

                if (adminInfo != null)
                {
                    adminInfo.MinSalary = (double)(decimal.TryParse(adminForm.MinSalary, out decimal monthlyIncome) ? monthlyIncome : 0);

                    TaskResult result = await _adminInfoService.UpdateAsync(adminInfo);
                    if (result.Succeeded)
                    {
                        return View(adminForm);
                    }

                    _logger.LogError(LogEvents.ListItems, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });
                }

                ModelState.AddModelError(string.Empty, "Alguma coisa deu errado!");
            }

            return View(adminForm);
        }
    }
}
