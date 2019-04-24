using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Store;
using LigaCancer.Models.FormModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin"), AutoValidateAntiforgeryToken]
    public class FamilyMemberController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IDataStore<FamilyMember> _familyMemberService;
        private readonly IDataStore<Patient> _patientService;

        public FamilyMemberController(IDataStore<FamilyMember> familyMemberService, UserManager<ApplicationUser> userManager, IDataStore<Patient> patientService)
        {
            _familyMemberService = familyMemberService;
            _userManager = userManager;
            _patientService = patientService;
        }

        [HttpGet]
        public IActionResult AddFamilyMember(string id)
        {
            FamilyMemberFormModel familyMemberForm = new FamilyMemberFormModel
            {
                PatientId = id
            };
            return PartialView("_AddFamilyMember", familyMemberForm);
        }

        [HttpPost]
        public async Task<IActionResult> AddFamilyMember(FamilyMemberFormModel familyMemberForm)
        {
            if (!ModelState.IsValid) return PartialView("_AddFamilyMember", familyMemberForm);

            ApplicationUser user = await _userManager.GetUserAsync(User);

            TaskResult result = await ((PatientStore)_patientService).AddFamilyMember(
                new FamilyMember
                {
                    Age = familyMemberForm.Age,
                    Kinship = familyMemberForm.Kinship,
                    MonthlyIncome = (double)familyMemberForm.MonthlyIncome,
                    Name = familyMemberForm.Name,
                    Sex = familyMemberForm.Sex,
                    UserCreated = user
                }, familyMemberForm.PatientId);

            if (result.Succeeded)
            {
                return StatusCode(200, "familyMember");
            }
            ModelState.AddErrors(result);

            return PartialView("_AddFamilyMember", familyMemberForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditFamilyMember(string id)
        {
            FamilyMemberFormModel familyMemberForm = new FamilyMemberFormModel();

            if (string.IsNullOrEmpty(id)) return PartialView("_EditFamilyMember", familyMemberForm);

            FamilyMember familyMember = await _familyMemberService.FindByIdAsync(id);
            if (familyMember != null)
            {
                familyMemberForm = new FamilyMemberFormModel
                {
                    Age = familyMember.Age,
                    Kinship = familyMember.Kinship,
                    MonthlyIncome = (decimal)familyMember.MonthlyIncome,
                    Name = familyMember.Name,
                    Sex = familyMember.Sex
                };
            }

            return PartialView("_EditFamilyMember", familyMemberForm);
        }

        [HttpPost]
        public async Task<IActionResult> EditFamilyMember(string id, FamilyMemberFormModel familyMemberForm)
        {
            if (!ModelState.IsValid) return PartialView("_EditFamilyMember", familyMemberForm);

            FamilyMember familyMember = await _familyMemberService.FindByIdAsync(id);
            TaskResult result = await ((FamilyMemberStore)_familyMemberService).UpdateFamilyIncomeByFamilyMember(familyMember);
            if (result.Succeeded)
            {
                ApplicationUser user = await _userManager.GetUserAsync(User);

                familyMember.Age = familyMemberForm.Age;
                familyMember.Kinship = familyMemberForm.Kinship;
                familyMember.MonthlyIncome = (double)familyMemberForm.MonthlyIncome;
                familyMember.Name = familyMemberForm.Name;
                familyMember.Sex = familyMemberForm.Sex;
                familyMember.UpdatedDate = DateTime.Now;
                familyMember.UserUpdated = user;

                result = await _familyMemberService.UpdateAsync(familyMember);
                if (result.Succeeded)
                {
                    return StatusCode(200, "familyMember");
                }
            }
            ModelState.AddErrors(result);

            return PartialView("_EditFamilyMember", familyMemberForm);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteFamilyMember(string id)
        {
            string name = string.Empty;

            if (string.IsNullOrEmpty(id)) return PartialView("_DeleteFamilyMember", name);

            FamilyMember familyMember = await _familyMemberService.FindByIdAsync(id);
            if (familyMember != null)
            {
                name = familyMember.Name;
            }

            return PartialView("_DeleteFamilyMember", name);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFamilyMember(string id, IFormCollection form)
        {
            if (string.IsNullOrEmpty(id)) return RedirectToAction("Index");

            FamilyMember familyMember = await _familyMemberService.FindByIdAsync(id);
            if (familyMember == null) return RedirectToAction("Index");

            TaskResult result = await _familyMemberService.DeleteAsync(familyMember);

            if (result.Succeeded)
            {
                return StatusCode(200, "familyMember");
            }
            ModelState.AddErrors(result);
            return PartialView("_DeleteFamilyMember", familyMember.Name);
        }

    }
}
