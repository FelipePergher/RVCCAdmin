// <copyright file="PatientApiController.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Data.Models.RelationModels;
using RVCC.Data.Repositories;
using RVCC.Models.SearchModel;
using RVCC.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RVCC.Controllers.Api
{
    [ApiController]
    public class PatientApiController : Controller
    {
        private readonly IDataRepository<Patient> _patientService;
        private readonly ILogger<PatientApiController> _logger;

        public PatientApiController(IDataRepository<Patient> patientService, ILogger<PatientApiController> logger)
        {
            _patientService = patientService;
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

                string[] includes =
                {
                    nameof(Patient.PatientInformation), nameof(Patient.Naturality), nameof(Patient.ActivePatient), nameof(Patient.Phones), nameof(Patient.Addresses),
                    nameof(PatientInformation.PatientInformationDoctors), nameof(PatientInformationDoctor.Doctor),
                    nameof(PatientInformation.PatientInformationCancerTypes), nameof(PatientInformationCancerType.CancerType),
                    nameof(PatientInformation.PatientInformationMedicines), nameof(PatientInformationMedicine.Medicine),
                    nameof(PatientInformation.PatientInformationTreatmentPlaces), nameof(PatientInformationTreatmentPlace.TreatmentPlace),
                };

                IEnumerable<Patient> patients = await _patientService.GetAllAsync(includes, sortColumn, sortDirection, patientSearchModel);

                IEnumerable<PatientViewModel> data = patients.Select(x => new PatientViewModel
                {
                    FirstName = x.FirstName,
                    LastName = x.Surname,
                    Rg = x.RG,
                    Cpf = x.CPF,
                    DateOfBirth = x.DateOfBirth.ToString("dd/MM/yyyy"),
                    JoinDate = x.JoinDate.ToString("dd/MM/yyyy"),
                    Phone = x.Phones.FirstOrDefault()?.Number,
                    Address = GetAddressToTable(x.Addresses.FirstOrDefault()),
                    TreatmentBeginDate = x.PatientInformation.TreatmentBeginDate == DateTime.MinValue ? string.Empty : x.PatientInformation.TreatmentBeginDate.ToString("dd/MM/yyyy"),
                    Medicines = string.Join(", ", x.PatientInformation.PatientInformationMedicines.Select(y => y.Medicine.Name).ToList()),
                    Canceres = string.Join(", ", x.PatientInformation.PatientInformationCancerTypes.Select(y => y.CancerType.Name).ToList()),
                    Doctors = string.Join(", ", x.PatientInformation.PatientInformationDoctors.Select(y => y.Doctor.Name).ToList()),
                    TreatmentPlaces = string.Join(", ", x.PatientInformation.PatientInformationTreatmentPlaces.Select(y => y.TreatmentPlace.City).ToList()),
                    Actions = GetActionsHtml(x, User)
                });

                int recordsTotal = _patientService.Count();
                int recordsFiltered = patients.Count();

                return Ok(new { searchModel.Draw, data = data.Skip(skip).Take(take), recordsTotal, recordsFiltered });
            }
            catch (Exception e)
            {
                _logger.LogError(LogEvents.ListItems, e, "Patient Search Error");
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

        private static string GetActionsHtml(Patient patient, ClaimsPrincipal user)
        {
            string options = string.Empty;

            if (!patient.ActivePatient.Death && !patient.ActivePatient.Discharge && !patient.ActivePatient.ResidenceChange)
            {
                string patientDetails = $@"<a href='/Patient/Details/{patient.PatientId}' class='dropdown-item' target='_blank'>
                                            <span class='fas fa-clipboard'></span> Detalhes do Paciente
                                        </a>";

                string socialObservation = $@"<a href='/Patient/AddSocialObservation/{patient.PatientId}' data-toggle='modal' data-target='#modal-action'
                                       data-title='Observações' class='dropdown-item socialObservationButton'><span class='fas fa-clipboard'></span> Observações</a>";

                string archivePatient = $"<a href='/Patient/ArchivePatient/{patient.PatientId}'' data-toggle='modal' data-target='#modal-action' " +
                    $"data-title='Arquivar Paciente' class='archivePatientButton dropdown-item'><span class='fas fa-user-alt-slash'></span> Arquivar </a>";

                options = patientDetails + socialObservation;

                if (!user.IsInRole(Roles.SocialAssistance))
                {
                    options += archivePatient;
                }
            }
            else if (!user.IsInRole(Roles.SocialAssistance))
            {
                string enablePatient = $"<a href='javascript:void(0);' data-url='/Patient/ActivePatient' data-id='{patient.PatientId}' " +
                    "data-title='Ativar Paciente' class='activePatientButton dropdown-item'><span class='fas fa-user-plus'></span> Reativar </a>";

                string deletePatient = $"<a href='javascript:void(0);' data-url='/Patient/DeletePatient' data-id='{patient.PatientId}' " +
                    "data-title='Deletar Paciente' class='deletePatientButton dropdown-item'><span class='fas fa-trash-alt'></span> Excluir </a>";

                if (!patient.ActivePatient.Death)
                {
                    options = enablePatient;
                }

                options += deletePatient;
            }

            string actionsHtml =
                "<div class='dropdown'>" +
                "  <button type='button' class='btn btn-info dropdown-toggle' data-toggle='dropdown'>Ações</button>" +
                "  <div class='dropdown-menu'>" +
                $"      {options}" +
                "  </div>" +
                "</div>";

            return actionsHtml;
        }

        private static string GetAddressToTable(Address address)
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
