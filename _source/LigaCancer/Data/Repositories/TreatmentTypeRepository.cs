// <copyright file="TreatmentTypeRepository.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Data.Repositories
{
    public class TreatmentTypeRepository : IDataRepository<TreatmentType>
    {
        private readonly ApplicationDbContext _context;

        public TreatmentTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.TreatmentTypes.Count();
        }

        public Task<TaskResult> CreateAsync(TreatmentType treatmentType)
        {
            var result = new TaskResult();
            try
            {
                _context.TreatmentTypes.Add(treatmentType);
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

        public Task<TaskResult> DeleteAsync(TreatmentType treatmentType)
        {
            var result = new TaskResult();
            try
            {
                _context.TreatmentTypes.Remove(treatmentType);
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

        public Task<TreatmentType> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<TreatmentType> query = _context.TreatmentTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.TreatmentTypeId == int.Parse(id)));
        }

        public Task<List<TreatmentType>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<TreatmentType> query = _context.TreatmentTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationTreatmentType(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredTreatmentTypes(query, (TreatmentTypeSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(TreatmentType treatmentType)
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

        public Task<TreatmentType> FindByNameAsync(string name, int treatmentTypeId = -1)
        {
            return Task.FromResult(_context.TreatmentTypes.FirstOrDefault(x => x.Name == name && x.TreatmentTypeId != treatmentTypeId));
        }

        public Task<List<TreatmentType>> GetNotRelatedToPatient(int patientId, int includeExpense = 0)
        {
            return Task.FromResult(_context.TreatmentTypes.AsNoTracking().Where(x => x.TreatmentTypeId == includeExpense || x.PatientTreatmentTypes.All(y => y.PatientId != patientId)).ToList());
        }

        #endregion

        #region Private Methods

        private static IQueryable<TreatmentType> GetOrdinationTreatmentType(IQueryable<TreatmentType> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Quantity" => sortDirection == "asc" ? query.OrderBy(x => x.PatientTreatmentTypes.Count) : query.OrderByDescending(x => x.PatientTreatmentTypes.Count),
                "Note" => sortDirection == "asc" ? query.OrderBy(x => x.Note) : query.OrderByDescending(x => x.Note),
                _ => sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };
        }

        private static IQueryable<TreatmentType> GetFilteredTreatmentTypes(IQueryable<TreatmentType> query, TreatmentTypeSearchModel treatmentTypeSearch)
        {
            if (!string.IsNullOrEmpty(treatmentTypeSearch.Name))
            {
                query = query.Where(x => x.Name.Contains(treatmentTypeSearch.Name));
            }

            if (!string.IsNullOrEmpty(treatmentTypeSearch.Note))
            {
                query = query.Where(x => x.Name.Contains(treatmentTypeSearch.Note));
            }

            return query;
        }

        #endregion
    }
}
