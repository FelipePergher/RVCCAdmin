// <copyright file="PatientRepository.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Data.Repositories
{
    public class PatientRepository : IDataRepository<Patient>
    {
        private readonly ApplicationDbContext _context;

        public PatientRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Patients.Count();
        }

        public Task<TaskResult> CreateAsync(Patient patient)
        {
            var result = new TaskResult();
            try
            {
                _context.Patients.Add(patient);
                _context.SaveChanges();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Errors.Add(new TaskError
                {
                    Code = e.HResult.ToString(),
                    Description = e.Message
                });
            }

            return Task.FromResult(result);
        }

        public Task<TaskResult> DeleteAsync(Patient patient)
        {
            var result = new TaskResult();
            try
            {
                _context.Patients.Remove(patient);
                _context.SaveChanges();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Errors.Add(new TaskError
                {
                    Code = e.HResult.ToString(),
                    Description = e.Message
                });
            }

            return Task.FromResult(result);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public Task<Patient> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<Patient> query = _context.Patients;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.PatientId == int.Parse(id)));
        }

        public Task<List<Patient>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Patient> query = _context.Patients;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationPatients(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredPatients(query, (PatientSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Patient patient)
        {
            var result = new TaskResult();
            try
            {
                patient.UpdatedTime = DateTime.Now;
                _context.SaveChanges();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Errors.Add(new TaskError
                {
                    Code = e.HResult.ToString(),
                    Description = e.Message
                });
            }

            return Task.FromResult(result);
        }

        #region Custom Methods

        public Task<Patient> FindByCpfAsync(string cpf, int patientId)
        {
            Patient patient = _context.Patients.FirstOrDefault(x => x.CPF == cpf && x.PatientId != patientId);
            return Task.FromResult(patient);
        }

        public Task<Patient> FindByRgAsync(string rg, int patientId)
        {
            Patient patient = _context.Patients.FirstOrDefault(x => x.RG == rg && x.PatientId != patientId);
            return Task.FromResult(patient);
        }

        public string GetPerCapitaIncome(List<FamilyMember> familyMembers, double monthlyPatient)
        {
            return familyMembers.Count > 0 ? ((familyMembers.Sum(x => x.MonthlyIncome) + monthlyPatient) / (familyMembers.Count + 1)).ToString("C2") : monthlyPatient.ToString("C2");
        }

        #endregion

        #region Private Methods

        private IQueryable<Patient> GetOrdinationPatients(IQueryable<Patient> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "LastName" => sortDirection == "asc"
                    ? query.OrderBy(x => x.Surname)
                    : query.OrderByDescending(x => x.Surname),
                "RG" => sortDirection == "asc" ? query.OrderBy(x => x.RG) : query.OrderByDescending(x => x.RG),
                "CPF" => sortDirection == "asc" ? query.OrderBy(x => x.CPF) : query.OrderByDescending(x => x.CPF),
                "DateOfBirth" => sortDirection == "asc"
                    ? query.OrderBy(x => x.DateOfBirth)
                    : query.OrderByDescending(x => x.DateOfBirth),
                "JoinDate" => sortDirection == "asc"
                    ? query.OrderBy(x => x.JoinDate)
                    : query.OrderByDescending(x => x.JoinDate),
                _ => sortDirection == "asc"
                    ? query.OrderBy(x => x.FirstName)
                    : query.OrderByDescending(x => x.FirstName)
            };
        }

        private IQueryable<Patient> GetFilteredPatients(IQueryable<Patient> query, PatientSearchModel patientSearch)
        {
            if (!string.IsNullOrEmpty(patientSearch.Name))
            {
                query = query.Where(x => x.FirstName.Contains(patientSearch.Name));
            }

            if (!string.IsNullOrEmpty(patientSearch.Surname))
            {
                query = query.Where(x => x.Surname.Contains(patientSearch.Surname));
            }

            if (!string.IsNullOrEmpty(patientSearch.Rg))
            {
                query = query.Where(x => x.RG.Contains(patientSearch.Rg));
            }

            if (!string.IsNullOrEmpty(patientSearch.Cpf))
            {
                query = query.Where(x => x.CPF.Contains(patientSearch.Cpf));
            }

            if (!string.IsNullOrEmpty(patientSearch.CivilState))
            {
                query = query.Where(x => x.CivilState == (Enums.CivilState)int.Parse(patientSearch.CivilState));
            }

            if (!string.IsNullOrEmpty(patientSearch.Sex))
            {
                query = query.Where(x => x.Sex == (Enums.Sex)int.Parse(patientSearch.Sex));
            }

            if (!string.IsNullOrEmpty(patientSearch.FamiliarityGroup))
            {
                query = query.Where(x => x.FamiliarityGroup == bool.Parse(patientSearch.FamiliarityGroup));
            }

            if (patientSearch.Death)
            {
                query = query.Where(x => x.ActivePatient.Death);
            }
            else if (patientSearch.Discharge)
            {
                query = query.Where(x => x.ActivePatient.Discharge);
            }
            else
            {
                query = query.Where(x => !x.ActivePatient.Discharge && !x.ActivePatient.Death);
            }

            foreach (string item in patientSearch.CancerTypes)
            {
                query = query.Where(x => x.PatientInformation.PatientInformationCancerTypes.FirstOrDefault(y => y.CancerTypeId == int.Parse(item)) != null);
            }

            foreach (string item in patientSearch.TreatmentPlaces)
            {
                query = query.Where(x => x.PatientInformation.PatientInformationTreatmentPlaces.FirstOrDefault(y => y.TreatmentPlaceId == int.Parse(item)) != null);
            }

            foreach (string item in patientSearch.Doctors)
            {
                query = query.Where(x => x.PatientInformation.PatientInformationDoctors.FirstOrDefault(y => y.DoctorId == int.Parse(item)) != null);
            }

            foreach (string item in patientSearch.Medicines)
            {
                query = query.Where(x => x.PatientInformation.PatientInformationMedicines.FirstOrDefault(y => y.MedicineId == int.Parse(item)) != null);
            }

            if (patientSearch.BirthdayDateFrom != null)
            {
                query = query.Where(x => x.DateOfBirth.Date >= DateTime.Parse(patientSearch.BirthdayDateFrom).Date);
            }

            if (patientSearch.BirthdayDateTo != null)
            {
                query = query.Where(x => x.DateOfBirth.Date <= DateTime.Parse(patientSearch.BirthdayDateTo).Date);
            }

            if (patientSearch.JoinDateFrom != null)
            {
                query = query.Where(x => x.JoinDate.Date >= DateTime.Parse(patientSearch.JoinDateFrom).Date);
            }

            if (patientSearch.JoinDateTo != null)
            {
                query = query.Where(x => x.JoinDate.Date <= DateTime.Parse(patientSearch.JoinDateTo).Date);
            }

            return query;
        }

        #endregion

    }
}
