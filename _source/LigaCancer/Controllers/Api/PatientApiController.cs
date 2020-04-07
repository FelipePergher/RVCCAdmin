using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.PatientModels;
using RVCC.Data.Repositories;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [ApiController]
    public class PatientApiController : Controller
    {
        private readonly IDataRepository<Patient> _patientService;
        private readonly IDataRepository<FamilyMember> _familyMemberService;
        private readonly ILogger<PatientApiController> _logger;

        public PatientApiController(IDataRepository<Patient> patientService, IDataRepository<FamilyMember> familyMemberService, ILogger<PatientApiController> logger)
        {
            _patientService = patientService;
            _familyMemberService = familyMemberService;
            _logger = logger;
        }

        [Authorize(Roles = Roles.AdminUserSocialAssistanceAuthorize)]
        [HttpPost("~/api/patient/search")]
        public async Task<IActionResult> PatientSearch([FromForm] SearchModel searchModel, [FromForm] PatientSearchModel patientSearchModel)
        {
            try
            {
                string sortColumn = searchModel.Columns[searchModel.Order[0].Column].Name;
                string sortDirection = searchModel.Order[0].Dir;
                int take = searchModel.Length != null ? int.Parse(searchModel.Length) : 0;
                int skip = searchModel.Start != null ? int.Parse(searchModel.Start) : 0;

                IEnumerable<Patient> patients = await _patientService.GetAllAsync(
                    new string[] {
                        "PatientInformation", "Naturality", "ActivePatient", "Phones", "Addresses",
                        "PatientInformation.PatientInformationDoctors", "PatientInformation.PatientInformationDoctors.Doctor",
                        "PatientInformation.PatientInformationCancerTypes", "PatientInformation.PatientInformationMedicines",
                        "PatientInformation.PatientInformationTreatmentPlaces", "PatientInformation.PatientInformationMedicines.Medicine",
                        "PatientInformation.PatientInformationCancerTypes.CancerType", "PatientInformation.PatientInformationTreatmentPlaces.TreatmentPlace"
                    }, sortColumn, sortDirection, patientSearchModel);

                IEnumerable<PatientViewModel> data = patients.Select(x => new PatientViewModel
                {
                    Status = x.ActivePatient.Discharge
                        ? Globals.GetDisplayName(Globals.ArchivePatientType.discharge)
                        : x.ActivePatient.Death ? Globals.GetDisplayName(Globals.ArchivePatientType.death) : "Ativo",
                    FirstName = x.FirstName,
                    LastName = x.Surname,
                    Rg = x.RG,
                    Cpf = x.CPF,
                    DateOfBirth = x.DateOfBirth.ToString("dd/MM/yyyy"),
                    JoinDate = x.JoinDate.ToString("dd/MM/yyyy"),
                    Sex = Globals.GetDisplayName(x.Sex),
                    Phone = x.Phones.FirstOrDefault()?.Number,
                    Address = GetAddressToTable(x.Addresses.FirstOrDefault()),
                    CivilState = Globals.GetDisplayName(x.CivilState),
                    FamiliarityGroup = x.FamiliarityGroup ? "<span class='fa fa-check'></span>" : "",
                    Profession = x.Profession,
                    Naturality = $"{x.Naturality.City} {x.Naturality.State} {x.Naturality.Country}",
                    PerCapitaIncome = ((PatientRepository)_patientService).GetPerCapitaIncome(
                        _familyMemberService.GetAllAsync(filter: new FamilyMemberSearchModel(x.PatientId.ToString())).Result, x.MonthlyIncome),
                    PerCapitaIncomeToSort = GetPerCapitaIncomeToSort(_familyMemberService.GetAllAsync(filter: new FamilyMemberSearchModel(x.PatientId.ToString())).Result, x.MonthlyIncome),
                    TreatmentBeginDate = x.PatientInformation.TreatmentBeginDate == DateTime.MinValue ? "" : x.PatientInformation.TreatmentBeginDate.ToString("dd/MM/yyyy"),
                    Medicines = string.Join(", ", x.PatientInformation.PatientInformationMedicines.Select(y => y.Medicine.Name).ToList()),
                    Canceres = string.Join(", ", x.PatientInformation.PatientInformationCancerTypes.Select(y => y.CancerType.Name).ToList()),
                    Doctors = string.Join(", ", x.PatientInformation.PatientInformationDoctors.Select(y => y.Doctor.Name).ToList()),
                    TreatmentPlaces = string.Join(", ", x.PatientInformation.PatientInformationTreatmentPlaces.Select(y => y.TreatmentPlace.City).ToList()),
                    Actions = GetActionsHtml(x)
                });

                if (sortColumn == "PerCapitaIncome")
                {
                    data = sortDirection == "asc"
                        ? data.OrderBy(x => x.PerCapitaIncomeToSort)
                        : data.OrderByDescending(x => x.PerCapitaIncomeToSort);
                }

                int recordsTotal = _patientService.Count();
                int recordsFiltered = patients.Count();

                return Ok(new { searchModel.Draw, data = data.Skip(skip).Take(take), recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Patient Search Error", null);
                return BadRequest();
            }
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpGet("~/api/patient/IsCpfExist")]
        public async Task<IActionResult> IsCpfExist(string cpf, int patientId)
        {
            Patient patient = await ((PatientRepository)_patientService).FindByCpfAsync(cpf, patientId);
            return Ok(patient == null);
        }

        [Authorize(Roles = Roles.AdminUserAuthorize)]
        [HttpGet("~/api/patient/IsRgExist")]
        public async Task<IActionResult> IsRgExist(string rg, int patientId)
        {
            Patient patient = await ((PatientRepository)_patientService).FindByRgAsync(rg, patientId);
            return Ok(patient == null);
        }

        #region Private Methods

        private string GetActionsHtml(Patient patient)
        {
            string options = string.Empty;
            if (!patient.ActivePatient.Death && !patient.ActivePatient.Discharge)
            {
                string editPatient = $"<a href='/Patient/EditPatientProfile/{patient.PatientId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Editar Paciente' class='dropdown-item editPatientButton'><i class='fas fa-clipboard'></i> Perfil do Paciente</a>";

                string editNaturality = $"<a href='/Patient/EditPatientNaturality/{patient.Naturality.NaturalityId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Editar Naturalidade' class='dropdown-item editNaturalityButton'><i class='fas fa-globe-americas'></i> Naturalidade</a>";

                string editPatientInformation = $"<a href='/Patient/EditPatientInformation/{patient.PatientInformation.PatientInformationId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Editar Informações do Paciente' class='dropdown-item editPatientInformationButton'><i class='fas fa-info'></i> Informação do Paciente</a>";

                string phones = $"<a href='/Phone/Index/{patient.PatientId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Telefones' class='dropdown-item phonesButton'><i class='fas fa-phone'></i> Telefones</a>";

                string addressses = $"<a href='/Address/Index/{patient.PatientId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Endereços' class='dropdown-item addressesButton'><i class='fas fa-address-book'></i> Endereços</a>";

                string familyMembers = $"<a href='/FamilyMember/Index/{patient.PatientId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Membros Familiares' class='dropdown-item familyMembersButton'><i class='fas fa-user-friends'></i> Membros Familiares</a>";

                string archivePatient = $"<a href='/Patient/ArchivePatient/{patient.PatientId}'' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Arquivar Paciente' class='archivePatientButton dropdown-item'><i class='fas fa-user-alt-slash'></i> Arquivar </a>";

                string fileUploadPatient = $"<a href='/FileAttachment/FileUpload/{patient.PatientId}'' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Arquivos' class='fileUploadPatientButton dropdown-item'><i class='fas fa-file-import'></i> Arquivos </a>";

                if (!User.IsInRole(Roles.SocialAssistance))
                {
                    options = editPatient + editNaturality + editPatientInformation + phones + addressses + familyMembers + fileUploadPatient + archivePatient;
                }
                else
                {
                    options = phones + addressses + familyMembers + fileUploadPatient;
                }
            }
            else if(!User.IsInRole(Roles.SocialAssistance))
            {
                string enablePatient = $"<a href='javascript:void(0);' data-url='/Patient/ActivePatient' data-id='{patient.PatientId}' " +
                    $"data-title='Ativar Paciente' class='activePatientButton dropdown-item'><i class='fas fa-user-plus'></i> Reativar </a>";
                string deletePatient = $"<a href='javascript:void(0);' data-url='/Patient/DeletePatient' data-id='{patient.PatientId}' " +
                $"data-title='Deletar Paciente' class='deletePatientButton dropdown-item'><i class='fas fa-trash-alt'></i> Excluir </a>";

                if (!patient.ActivePatient.Death)
                {
                    options = enablePatient;
                }

                options += deletePatient;
            }

            string actionsHtml =
                $"<div class='dropdown'>" +
                $"  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                $"  <div class='dropdown-menu'>" +
                $"      {options}" +
                $"  </div>" +
                $"</div>";

            return actionsHtml;
        }

        private double GetPerCapitaIncomeToSort(List<FamilyMember> familyMembers, double montlhyPatient)
        {
            return familyMembers.Count > 0 ? ((familyMembers.Sum(x => x.MonthlyIncome) + montlhyPatient) / (familyMembers.Count + 1)) : montlhyPatient;
        }

        private string GetAddressToTable(Address address)
        {
            string addressString = string.Empty;

            if (address != null)
            {
                addressString = $"{address.Street} {address.Neighborhood} {address.HouseNumber} {address.City}";
            }

            return addressString;
        }

        #endregion
    }
}
