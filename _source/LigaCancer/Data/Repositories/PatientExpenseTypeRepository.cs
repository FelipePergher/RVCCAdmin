// <copyright file="PatientExpenseTypeRepository.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
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
    public class PatientExpenseTypeRepository : IDataRepository<PatientExpenseType>
    {
        private readonly ApplicationDbContext _context;

        public PatientExpenseTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.PatientExpenseTypes.Count();
        }

        public int CountByPatient(int patientId)
        {
            return _context.PatientExpenseTypes.Count(x => x.PatientId == patientId);
        }

        public Task<PatientExpenseType> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<PatientExpenseType> query = _context.PatientExpenseTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.PatientExpenseTypeId == int.Parse(id)));
        }

        public Task<TaskResult> CreateAsync(PatientExpenseType patientExpenseType)
        {
            var result = new TaskResult();
            try
            {
                _context.PatientExpenseTypes.Add(patientExpenseType);
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

        public Task<TaskResult> DeleteAsync(PatientExpenseType patientExpenseType)
        {
            var result = new TaskResult();
            try
            {
                _context.PatientExpenseTypes.Remove(patientExpenseType);
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

        public Task<List<PatientExpenseType>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<PatientExpenseType> query = _context.PatientExpenseTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationPatientExpenseTypes(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredPatientExpenseTypes(query, (PatientExpenseTypeSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(PatientExpenseType patientExpenseType)
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

        #region Private Methods

        private static IQueryable<PatientExpenseType> GetOrdinationPatientExpenseTypes(IQueryable<PatientExpenseType> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "ExpenseType" => sortDirection == "asc"
                    ? query.OrderBy(x => x.ExpenseType.Name)
                    : query.OrderByDescending(x => x.ExpenseType.Name),
                _ => sortDirection == "asc"
                    ? query.OrderBy(x => x.Patient.FirstName + x.Patient.Surname)
                    : query.OrderByDescending(x => x.Patient.FirstName + x.Patient.Surname)
            };
        }

        private static IQueryable<PatientExpenseType> GetFilteredPatientExpenseTypes(IQueryable<PatientExpenseType> query, PatientExpenseTypeSearchModel patientExpenseTypeSearch)
        {
            if (!string.IsNullOrEmpty(patientExpenseTypeSearch.PatientId))
            {
                query = query.Where(x => x.PatientId == int.Parse(patientExpenseTypeSearch.PatientId));
            }

            if (!string.IsNullOrEmpty(patientExpenseTypeSearch.Name))
            {
                query = query.Where(x => x.Patient.FirstName.Contains(patientExpenseTypeSearch.Name) || x.Patient.Surname.Contains(patientExpenseTypeSearch.Name));
            }

            if (!string.IsNullOrEmpty(patientExpenseTypeSearch.ExpenseType))
            {
                query = query.Where(x => x.ExpenseType.Name.Contains(patientExpenseTypeSearch.ExpenseType));
            }

            return query;
        }

        #endregion
    }
}
