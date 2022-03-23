// <copyright file="AdminController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Data.Repositories;
using RVCC.Models.FormModel;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers
{
    [Authorize(Roles = Roles.Admin)]
    [AutoValidateAntiforgeryToken]
    public class AdminController : Controller
    {
        private readonly IDataRepository<Setting> _settingRepository;
        private readonly ILogger<AdminController> _logger;

        public AdminController(IDataRepository<Setting> settingRepository, ILogger<AdminController> logger)
        {
            _settingRepository = settingRepository;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var minSalary = ((SettingRepository)_settingRepository).GetByKey(SettingKey.MinSalary).GetValueAsDecimal();

            var adminForm = new AdminFormModel
            {
                MinSalary = minSalary.ToString("N2")
            };

            return View(adminForm);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AdminFormModel adminForm)
        {
            if (ModelState.IsValid)
            {
                // Min salary config
                TaskResult result;
                var minSalary = (double)(decimal.TryParse(adminForm.MinSalary, out decimal monthlyIncome) ? monthlyIncome : 0);
                Setting minSalarySettingDb = ((SettingRepository)_settingRepository).GetByKey(SettingKey.MinSalary);
                if (minSalarySettingDb == null)
                {
                    var minSalarySetting = new Setting(SettingKey.MinSalary, minSalary.ToString("N2"));
                    result = await _settingRepository.CreateAsync(minSalarySetting);
                }
                else
                {
                    minSalarySettingDb.Value = minSalary.ToString("N2");
                    result = await _settingRepository.UpdateAsync(minSalarySettingDb);
                }

                if (result.Succeeded)
                {
                    return View(adminForm);
                }

                _logger.LogError(LogEvents.ListItems, "Errors: {errorList}", new { errorList = string.Join(" || ", result.Errors.Select(x => $"{x.Code} {x.Description}")) });

                ModelState.AddModelError(string.Empty, "Alguma coisa deu errado!");
            }

            return View(adminForm);
        }
    }
}
