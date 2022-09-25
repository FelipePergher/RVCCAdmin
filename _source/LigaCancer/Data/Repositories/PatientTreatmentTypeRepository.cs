// <copyright file="PatientTreatmentTypeRepository.cs" company="Doffs">
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
    public class PatientTreatmentTypeRepository : IDataRepository<PatientTreatmentType>
    {
        private readonly ApplicationDbContext _context;

        public PatientTreatmentTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.PatientTreatmentTypes.Count();
        }

        public int CountByPatient(int patientId)
        {
            return _context.PatientTreatmentTypes.Count(x => x.PatientId == patientId);
        }

        public Task<PatientTreatmentType> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<PatientTreatmentType> query = _context.PatientTreatmentTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.PatientTreatmentTypeId == int.Parse(id)));
        }

        public Task<TaskResult> CreateAsync(PatientTreatmentType patientTreatmentType)
        {
            var result = new TaskResult();
            try
            {
                _context.PatientTreatmentTypes.Add(patientTreatmentType);
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

        public Task<TaskResult> DeleteAsync(PatientTreatmentType patientTreatmentType)
        {
            var result = new TaskResult();
            try
            {
                _context.PatientTreatmentTypes.Remove(patientTreatmentType);
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

        public Task<List<PatientTreatmentType>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<PatientTreatmentType> query = _context.PatientTreatmentTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationPatientTreatmentTypes(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredPatientTreatmentTypes(query, (PatientTreatmentTypeSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(PatientTreatmentType patientTreatmentType)
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

        private static IQueryable<PatientTreatmentType> GetOrdinationPatientTreatmentTypes(IQueryable<PatientTreatmentType> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Note" => sortDirection == "asc"
                    ? query.OrderBy(x => x.TreatmentType.Note)
                    : query.OrderByDescending(x => x.TreatmentType.Note),
                "StartDate" => sortDirection == "asc"
                    ? query.OrderBy(x => x.StartDate)
                    : query.OrderByDescending(x => x.StartDate),
                "EndDate" => sortDirection == "asc"
                    ? query.OrderBy(x => x.EndDate)
                    : query.OrderByDescending(x => x.EndDate),
                _ => sortDirection == "asc"
                    ? query.OrderBy(x => x.TreatmentType.Name)
                    : query.OrderByDescending(x => x.TreatmentType.Name),
            };
        }

        private static IQueryable<PatientTreatmentType> GetFilteredPatientTreatmentTypes(IQueryable<PatientTreatmentType> query, PatientTreatmentTypeSearchModel patientTreatmentTypeSearch)
        {
            if (!string.IsNullOrEmpty(patientTreatmentTypeSearch.PatientId))
            {
                query = query.Where(x => x.PatientId == int.Parse(patientTreatmentTypeSearch.PatientId));
            }

            return query;
        }

        #endregion
    }
}
