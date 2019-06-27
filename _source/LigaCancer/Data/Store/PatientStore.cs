using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.SearchModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Store
{
    public class PatientStore : IDataStore<Patient>
    {
        private readonly ApplicationDbContext _context;

        public PatientStore(ApplicationDbContext context)
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
                query = GetOrdenationPatients(query, sortColumn, sortDirection);
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

        public string GetPerCapitaIncome(List<FamilyMember> familyMembers, double montlhyPatient)
        {
            return familyMembers.Count > 0 ? ((familyMembers.Sum(x => x.MonthlyIncome) + montlhyPatient) / (familyMembers.Count + 1)).ToString("C2") : montlhyPatient.ToString("C2");
        }

        #endregion

        #region Private Methods

        private IQueryable<Patient> GetOrdenationPatients(IQueryable<Patient> query, string sortColumn, string sortDirection)
        {
            switch (sortColumn)
            {
                case "FirstName":
                    return sortDirection == "asc" ? query.OrderBy(x => x.FirstName) : query.OrderByDescending(x => x.FirstName);
                case "LastName":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Surname) : query.OrderByDescending(x => x.Surname);
                case "RG":
                    return sortDirection == "asc" ? query.OrderBy(x => x.RG) : query.OrderByDescending(x => x.RG);
                case "CPF":
                    return sortDirection == "asc" ? query.OrderBy(x => x.CPF) : query.OrderByDescending(x => x.CPF);
                case "DateOfBirth":
                    return sortDirection == "asc" ? query.OrderBy(x => x.DateOfBirth) : query.OrderByDescending(x => x.DateOfBirth);
                case "Sex":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Sex) : query.OrderByDescending(x => x.Sex);
                case "CivilState":
                    return sortDirection == "asc" ? query.OrderBy(x => x.CivilState) : query.OrderByDescending(x => x.CivilState);
                case "FamiliarityGroup":
                    return sortDirection == "asc" ? query.OrderBy(x => x.FamiliarityGroup) : query.OrderByDescending(x => x.FamiliarityGroup);
                case "Profession":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Profession) : query.OrderByDescending(x => x.Profession);
                default:
                    return sortDirection == "asc" ? query.OrderBy(x => x.FirstName) : query.OrderByDescending(x => x.FirstName);
            }
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
                query = query.Where(x => x.CivilState == (Globals.CivilState)int.Parse(patientSearch.CivilState));
            }

            if (!string.IsNullOrEmpty(patientSearch.Sex))
            {
                query = query.Where(x => x.Sex == (Globals.Sex)int.Parse(patientSearch.Sex));
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

            return query;
        }

        #endregion
    }
}
