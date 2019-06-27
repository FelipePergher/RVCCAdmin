using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Store;
using LigaCancer.Models.SearchModel;
using LigaCancer.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin, User")]
    [ApiController]
    public class PatientApiController : Controller
    {
        private readonly IDataStore<Patient> _patientService;
        private readonly IDataStore<FamilyMember> _familyMemberService;
        private readonly ILogger<PatientApiController> _logger;

        public PatientApiController(IDataStore<Patient> patientService, IDataStore<FamilyMember> familyMemberService, ILogger<PatientApiController> logger)
        {
            _patientService = patientService;
            _familyMemberService = familyMemberService;
            _logger = logger;
        }

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
                    DateOfBirth = x.DateOfBirth.ToString(),
                    Sex = Globals.GetDisplayName(x.Sex),
                    Phone = x.Phones.FirstOrDefault()?.Number,
                    Address = GetAddressToTable(x.Addresses.FirstOrDefault()),
                    CivilState = Globals.GetDisplayName(x.CivilState),
                    FamiliarityGroup = x.FamiliarityGroup ? "<span class='fa fa-check'></span>" : "",
                    Profession = x.Profession,
                    PerCapitaIncome = ((PatientStore)_patientService).GetPerCapitaIncome(
                        _familyMemberService.GetAllAsync(filter: new FamilyMemberSearchModel(x.PatientId.ToString())).Result, x.MonthlyIncome),
                    PerCapitaIncomeToSort = GetPerCapitaIncomeToSort(_familyMemberService.GetAllAsync(filter: new FamilyMemberSearchModel(x.PatientId.ToString())).Result, x.MonthlyIncome),
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

        [HttpGet("~/api/patient/IsCpfExist")]
        public async Task<IActionResult> IsCpfExist(string cpf, int patientId)
        {
            Patient patient = await ((PatientStore)_patientService).FindByCpfAsync(cpf, patientId);
            return Ok(patient == null);
        }

        [HttpGet("~/api/patient/IsRgExist")]
        public async Task<IActionResult> IsRgExist(string rg, int patientId)
        {
            Patient patient = await ((PatientStore)_patientService).FindByRgAsync(rg, patientId);
            return Ok(patient == null);
        }

        #region Private Methods

        private string GetActionsHtml(Patient patient)
        {
            string options = string.Empty;
            if (!patient.ActivePatient.Death && !patient.ActivePatient.Discharge)
            {
                string editPatient = $"<a href='/Admin/Patient/EditPatientProfile/{patient.PatientId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Editar Paciente' class='dropdown-item editPatientButton'><i class='fas fa-clipboard'></i> Perfil do Paciente</a>";

                string editNaturality = $"<a href='/Admin/Patient/EditPatientNaturality/{patient.Naturality.NaturalityId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Editar Naturalidade' class='dropdown-item editNaturalityButton'><i class='fas fa-globe-americas'></i> Naturalidade</a>";

                string editPatientInformation = $"<a href='/Admin/Patient/EditPatientInformation/{patient.PatientInformation.PatientInformationId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Editar Informação do Paciente' class='dropdown-item editPatientInformationButton'><i class='fas fa-info'></i> Informação do Paciente</a>";

                string phones = $"<a href='/Admin/Phone/Index/{patient.PatientId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Telefones' class='dropdown-item phonesButton'><i class='fas fa-phone'></i> Telefones</a>";

                string addressses = $"<a href='/Admin/Address/Index/{patient.PatientId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Endereços' class='dropdown-item addressesButton'><i class='fas fa-address-book'></i> Endereços</a>";

                string familyMembers = $"<a href='/Admin/FamilyMember/Index/{patient.PatientId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Membros Familiares' class='dropdown-item familyMembersButton'><i class='fas fa-user-friends'></i> Membros Familiares</a>";

                string archivePatient = $"<a href='/Admin/Patient/ArchivePatient/{patient.PatientId}'' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Arquivar Paciente' class='archivePatientButton dropdown-item'><i class='fas fa-user-alt-slash'></i> Arquivar </a>";

                string fileUploadPatient = $"<a href='/Admin/FileAttachment/FileUpload/{patient.PatientId}'' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Arquivos' class='fileUploadPatientButton dropdown-item'><i class='fas fa-file-import'></i> Arquivos </a>";

                options = editPatient + editNaturality + editPatientInformation + phones + addressses + familyMembers + fileUploadPatient + archivePatient;
            }
            else
            {
                string enablePatient = $"<a href='javascript:void(0);' data-url='/Admin/Patient/ActivePatient' data-id='{patient.PatientId}' " +
                    $"data-title='Ativar Paciente' class='activePatientButton dropdown-item'><i class='fas fa-user-plus'></i> Reativar </a>";
                string deletePatient = $"<a href='javascript:void(0);' data-url='/Admin/Patient/DeletePatient' data-id='{patient.PatientId}' " +
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
