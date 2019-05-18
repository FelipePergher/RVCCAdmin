using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Store;
using LigaCancer.Models.SearchModel;
using LigaCancer.Models.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    public class PatientApiController : Controller
    {
        private readonly IDataStore<Patient> _patientService;
        private readonly IDataStore<FamilyMember> _familyMemberService;

        public PatientApiController(IDataStore<Patient> patientService, IDataStore<FamilyMember> familyMemberService)
        {
            _patientService = patientService;
            _familyMemberService = familyMemberService;
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
                        "PatientInformation", "Naturality", "ActivePatient", "PatientInformation.PatientInformationDoctors", "PatientInformation.PatientInformationDoctors.Doctor",
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

                if(sortColumn == "PerCapitaIncome")
                {
                    data = sortDirection == "asc" 
                        ? data.OrderBy(x => x.PerCapitaIncomeToSort) 
                        : data.OrderByDescending(x => x.PerCapitaIncomeToSort);
                }

                int recordsTotal = _patientService.Count();
                int recordsFiltered = patients.Count();

                return Ok(new { searchModel.Draw, data = data.Skip(skip).Take(take), recordsTotal, recordsFiltered });
            }
            catch
            {
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
                string editPatient = $"<a href='/Patient/EditPatientProfile/{patient.PatientId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Editar Paciente' class='dropdown-item editPatientButton'><i class='fas fa-clipboard'></i> Perfil do Paciente</a>";

                string editNaturality = $"<a href='/Patient/EditPatientNaturality/{patient.Naturality.NaturalityId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Editar Naturalidade' class='dropdown-item editNaturalityButton'><i class='fas fa-globe-americas'></i> Naturalidade</a>";

                string editPatientInformation = $"<a href='/Patient/EditPatientInformation/{patient.PatientInformation.PatientInformationId}' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Editar Informação do Paciente' class='dropdown-item editPatientInformationButton'><i class='fas fa-info'></i> Informação do Paciente</a>";

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

                options = editPatient + editNaturality + editPatientInformation + phones + addressses + familyMembers + fileUploadPatient + archivePatient ;
            }
            else
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

        #endregion
    }
}
