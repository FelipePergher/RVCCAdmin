using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Store;
using LigaCancer.Models.FormModel;
using LigaCancer.Models.SearchModel;
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
        public IActionResult Index(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            return PartialView("Partials/_Index", new FamilyMemberSearchModel(id));
        }

        [HttpGet]
        public IActionResult AddFamilyMember(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();
            return PartialView("Partials/_AddFamilyMember", new FamilyMemberFormModel());
        }

        [HttpPost]
        public async Task<IActionResult> AddFamilyMember(string id, FamilyMemberFormModel familyMemberForm)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            if (ModelState.IsValid)
            {
                if (await _patientService.FindByIdAsync(id) == null) return NotFound();

                FamilyMember familyMember = new FamilyMember
                {
                    PatientId = int.Parse(id),
                    DateOfBirth = familyMemberForm.DateOfBirth,
                    Kinship = familyMemberForm.Kinship,
                    MonthlyIncome = (double)familyMemberForm.MonthlyIncome,
                    Name = familyMemberForm.Name,
                    Sex = familyMemberForm.Sex,
                    UserCreated = await _userManager.GetUserAsync(User)
                };

                TaskResult result = await _familyMemberService.CreateAsync(familyMember);

                if (result.Succeeded) return Ok();
                return BadRequest(result.Errors);
            }

            return PartialView("Partials/_AddFamilyMember", familyMemberForm);
        }

        [HttpGet]
        public async Task<IActionResult> EditFamilyMember(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            FamilyMember familyMember = await _familyMemberService.FindByIdAsync(id);

            if (familyMember == null) return NotFound();

            return PartialView("Partials/_EditFamilyMember", new FamilyMemberFormModel
            {
                DateOfBirth = familyMember.DateOfBirth,
                Kinship = familyMember.Kinship,
                MonthlyIncome = (decimal)familyMember.MonthlyIncome,
                Name = familyMember.Name,
                Sex = familyMember.Sex
            });
        }

        [HttpPost]
        public async Task<IActionResult> EditFamilyMember(string id, FamilyMemberFormModel familyMemberForm)
        {
            if (ModelState.IsValid)
            {
                FamilyMember familyMember = await _familyMemberService.FindByIdAsync(id);
            
                familyMember.DateOfBirth = familyMemberForm.DateOfBirth;
                familyMember.Kinship = familyMemberForm.Kinship;
                familyMember.MonthlyIncome = (double)familyMemberForm.MonthlyIncome;
                familyMember.Name = familyMemberForm.Name;
                familyMember.Sex = familyMemberForm.Sex;
                familyMember.UpdatedDate = DateTime.Now;
                familyMember.UserUpdated = await _userManager.GetUserAsync(User);

                TaskResult result = await _familyMemberService.UpdateAsync(familyMember);

                if (result.Succeeded) return Ok();
                return BadRequest(result.Errors);
            }
            return PartialView("Partials/_EditFamilyMember", familyMemberForm);
        }

        [HttpPost, IgnoreAntiforgeryToken]
        public async Task<IActionResult> DeleteFamilyMember(string id)
        {
            if (string.IsNullOrEmpty(id)) return BadRequest();

            FamilyMember familyMember = await _familyMemberService.FindByIdAsync(id);

            if (familyMember == null) return NotFound();

            TaskResult result = await _familyMemberService.DeleteAsync(familyMember);

            if (result.Succeeded) return Ok();
            return BadRequest(result.Errors);
        }

    }
}
