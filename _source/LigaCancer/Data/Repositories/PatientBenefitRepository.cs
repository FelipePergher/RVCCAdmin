﻿using Microsoft.EntityFrameworkCore;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.RelationModels;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Data.Repositories
{
    public class PatientBenefitRepository : IDataRepository<PatientBenefit>
    {
        private readonly ApplicationDbContext _context;

        public PatientBenefitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.PatientBenefits.Count();
        }

        public Task<PatientBenefit> FindByIdAsync(string id, string[] includes = null)
        {
            throw new NotImplementedException();
        }

        public Task<TaskResult> CreateAsync(PatientBenefit patientBenefit)
        {
            var result = new TaskResult();
            try
            {
                _context.PatientBenefits.Add(patientBenefit);
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

        public Task<TaskResult> DeleteAsync(PatientBenefit patientBenefit)
        {
            var result = new TaskResult();
            try
            {
                _context.PatientBenefits.Remove(patientBenefit);
                _context.SaveChanges();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Errors.Add(new TaskError
                {
                    Code = e.HResult.ToString(),
                    Description = e.Message
                }); ;
            }

            return Task.FromResult(result);
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public Task<PatientBenefit> FindByIdsAsync(int patientId, int benefitId, string[] includes = null)
        {
            IQueryable<PatientBenefit> query = _context.PatientBenefits;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.PatientId == patientId && x.BenefitId == benefitId));
        }

        public Task<List<PatientBenefit>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<PatientBenefit> query = _context.PatientBenefits;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationPatientBenefits(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredPatientBenefits(query, (PatientBenefitSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(PatientBenefit model)
        {
            var result = new TaskResult();
            try
            {
                model.UpdatedTime = DateTime.Now;
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

        #region Private Methods

        private IQueryable<PatientBenefit> GetOrdinationPatientBenefits(IQueryable<PatientBenefit> query, string sortColumn, string sortDirection)
        {
            switch (sortColumn)
            {
                case "Benefit":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Benefit.Name) : query.OrderByDescending(x => x.Benefit.Name);
                case "Date":
                    return sortDirection == "asc" ? query.OrderBy(x => x.BenefitDate) : query.OrderByDescending(x => x.BenefitDate);
                case "Quantity":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Quantity) : query.OrderByDescending(x => x.Quantity);
                default:
                    return sortDirection == "asc"
                        ? query.OrderBy(x => x.Patient.FirstName + x.Patient.Surname)
                        : query.OrderByDescending(x => x.Patient.FirstName + x.Patient.Surname);
            }
        }

        private IQueryable<PatientBenefit> GetFilteredPatientBenefits(IQueryable<PatientBenefit> query, PatientBenefitSearchModel patientBenefitSearch)
        {
            if (!string.IsNullOrEmpty(patientBenefitSearch.Name))
            {
                query = query.Where(x => x.Patient.FirstName.Contains(patientBenefitSearch.Name) || x.Patient.Surname.Contains(patientBenefitSearch.Name));
            }

            if (!string.IsNullOrEmpty(patientBenefitSearch.Benefit))
            {
                query = query.Where(x => x.Benefit.Name.Contains(patientBenefitSearch.Benefit) || x.Benefit.Name.Contains(patientBenefitSearch.Benefit));
            }

            if (patientBenefitSearch.DateFrom != null)
            {
                query = query.Where(x => x.BenefitDate.Date >= DateTime.Parse(patientBenefitSearch.DateFrom).Date);
            }

            if (patientBenefitSearch.DateTo != null)
            {
                query = query.Where(x => x.BenefitDate.Date <= DateTime.Parse(patientBenefitSearch.DateTo).Date);
            }

            return query;
        }

        #endregion
    }
}