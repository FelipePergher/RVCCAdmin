using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.FormModel;
using LigaCancer.Models.SearchModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin, User")]
    [AutoValidateAntiforgeryToken]
    [Area("Admin")]
    public class CancerTypeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<CancerType> _cancerTypeService;
        private readonly ILogger<CancerTypeController> _logger;

        public CancerTypeController(
            IDataStore<CancerType> cancerTypeService,
            ILogger<CancerTypeController> logger,
            UserManager<ApplicationUser> userManager)
        {
            _cancerTypeService = cancerTypeService;
            _userManager = userManager;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(new CancerTypeSearchModel());
        }

        [HttpGet]
        public IActionResult AddCancerType()
        {
            return PartialView("Partials/_AddCancerType", new CancerTypeFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddCancerType(CancerTypeFormModel cancerTypeForm)
        {
            if (ModelState.IsValid)
            {
                var cancerType = new CancerType(cancerTypeForm.Name, await _userManager.GetUserAsync(User));

                TaskResult result = await _cancerTypeService.CreateAsync(cancerType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_AddCancerType", cancerTypeForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditCancerType(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);

            if (cancerType == null)
            {
                return NotFound();
            }

            var cancerTypeForm = new CancerTypeFormModel(cancerType.Name, cancerType.CancerTypeId);

            return PartialView("Partials/_EditCancerType", cancerTypeForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditCancerType(string id, CancerTypeFormModel cancerTypeForm)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);

                if (cancerType == null)
                {
                    return NotFound();
                }

                cancerType.Name = cancerTypeForm.Name;
                cancerType.UpdatedDate = DateTime.Now;
                cancerType.UserUpdated = await _userManager.GetUserAsync(User);

                TaskResult result = await _cancerTypeService.UpdateAsync(cancerType);
                if (result.Succeeded)
                {
                    return Ok();
                }

                _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
                return BadRequest();
            }

            return PartialView("Partials/_EditCancerType", cancerTypeForm);
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteCancerType([FromForm] string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return BadRequest();
            }

            CancerType cancerType = await _cancerTypeService.FindByIdAsync(id);

            if (cancerType == null)
            {
                return NotFound();
            }

            TaskResult result = await _cancerTypeService.DeleteAsync(cancerType);

            if (result.Succeeded)
            {
                return Ok();
            }

            _logger.LogError(string.Join(" || ", result.Errors.Select(x => x.ToString())));
            return BadRequest(result);
        }

    }
}
