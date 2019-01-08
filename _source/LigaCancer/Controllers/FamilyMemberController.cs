using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.MedicalViewModels;
using LigaCancer.Code;
using LigaCancer.Data.Store;
using LigaCancer.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using LigaCancer.Code.Interface;

namespace LigaCancer.Controllers
{
    [Authorize(Roles = "Admin")]
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


        public IActionResult AddFamilyMember(string id)
        {
            FamilyMemberViewModel familyMemberViewModel = new FamilyMemberViewModel
            {
                PatientId = id
            };
            return PartialView("_AddFamilyMember", familyMemberViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddFamilyMember(FamilyMemberViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_AddFamilyMember", model);

            ApplicationUser user = await _userManager.GetUserAsync(this.User);

            TaskResult result = await ((PatientStore)_patientService).AddFamilyMember(
                new FamilyMember
                {
                    Age = model.Age,
                    Kinship = model.Kinship,
                    MonthlyIncome = (double)model.MonthlyIncome,
                    Name = model.Name,
                    Sex = model.Sex,
                    UserCreated = user
                }, model.PatientId);

            if (result.Succeeded)
            {
                return StatusCode(200, "familyMember");
            }
            ModelState.AddErrors(result);

            return PartialView("_AddFamilyMember", model);
        }


        public async Task<IActionResult> EditFamilyMember(string id)
        {
            FamilyMemberViewModel familyMemberViewModel = new FamilyMemberViewModel();

            if (string.IsNullOrEmpty(id)) return PartialView("_EditFamilyMember", familyMemberViewModel);

            FamilyMember familyMember = await _familyMemberService.FindByIdAsync(id);
            if (familyMember != null)
            {
                familyMemberViewModel = new FamilyMemberViewModel
                {
                    Age = familyMember.Age,
                    Kinship = familyMember.Kinship,
                    MonthlyIncome = (decimal)familyMember.MonthlyIncome,
                    Name = familyMember.Name,
                    Sex = familyMember.Sex
                };
            }

            return PartialView("_EditFamilyMember", familyMemberViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFamilyMember(string id, FamilyMemberViewModel model)
        {
            if (!ModelState.IsValid) return PartialView("_EditFamilyMember", model);

            FamilyMember familyMember = await _familyMemberService.FindByIdAsync(id);
            TaskResult result = await ((FamilyMemberStore)_familyMemberService).UpdateFamilyIncomeByFamilyMember(familyMember);
            if (result.Succeeded)
            {
                ApplicationUser user = await _userManager.GetUserAsync(this.User);

                familyMember.Age = model.Age;
                familyMember.Kinship = model.Kinship;
                familyMember.MonthlyIncome = (double)model.MonthlyIncome;
                familyMember.Name = model.Name;
                familyMember.Sex = model.Sex;
                familyMember.LastUpdatedDate = DateTime.Now;
                familyMember.LastUserUpdate = user;

                result = await _familyMemberService.UpdateAsync(familyMember);
                if (result.Succeeded)
                {
                    return StatusCode(200, "familyMember");
                }
            }
            ModelState.AddErrors(result);

            return PartialView("_EditFamilyMember", model);
        }


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
        [ValidateAntiForgeryToken]
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
