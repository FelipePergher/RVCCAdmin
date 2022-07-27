// <copyright file="AuxiliarAccessoryTypeRepository.cs" company="Doffs">
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
    public class AuxiliarAccessoryTypeRepository : IDataRepository<AuxiliarAccessoryType>
    {
        private readonly ApplicationDbContext _context;

        public AuxiliarAccessoryTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.AuxiliarAccessoryTypes.Count();
        }

        public Task<TaskResult> CreateAsync(AuxiliarAccessoryType auxiliarAccessoryType)
        {
            var result = new TaskResult();
            try
            {
                _context.AuxiliarAccessoryTypes.Add(auxiliarAccessoryType);
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

        public Task<TaskResult> DeleteAsync(AuxiliarAccessoryType auxiliarAccessoryType)
        {
            var result = new TaskResult();
            try
            {
                _context.AuxiliarAccessoryTypes.Remove(auxiliarAccessoryType);
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

        public Task<AuxiliarAccessoryType> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<AuxiliarAccessoryType> query = _context.AuxiliarAccessoryTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.AuxiliarAccessoryTypeId == int.Parse(id)));
        }

        public Task<List<AuxiliarAccessoryType>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<AuxiliarAccessoryType> query = _context.AuxiliarAccessoryTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationAuxiliarAccessoryType(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredAuxiliarAccessoryTypes(query, (AuxiliarAccessoryTypeSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(AuxiliarAccessoryType auxiliarAccessoryType)
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

        public Task<AuxiliarAccessoryType> FindByNameAsync(string name, int auxiliarAccessoryTypeId = -1)
        {
            return Task.FromResult(_context.AuxiliarAccessoryTypes.FirstOrDefault(x => x.Name == name && x.AuxiliarAccessoryTypeId != auxiliarAccessoryTypeId));
        }

        public Task<List<AuxiliarAccessoryType>> GetNotRelatedToPatient(int patientId, int includeExpense = 0)
        {
            return Task.FromResult(_context.AuxiliarAccessoryTypes.AsNoTracking().Where(x => x.AuxiliarAccessoryTypeId == includeExpense || x.PatientAuxiliarAccessoryTypes.All(y => y.PatientId != patientId)).ToList());
        }

        #endregion

        #region Private Methods

        private static IQueryable<AuxiliarAccessoryType> GetOrdinationAuxiliarAccessoryType(IQueryable<AuxiliarAccessoryType> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Quantity" => sortDirection == "asc" ? query.OrderBy(x => x.PatientAuxiliarAccessoryTypes.Count) : query.OrderByDescending(x => x.PatientAuxiliarAccessoryTypes.Count),
                _ => sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };
        }

        private static IQueryable<AuxiliarAccessoryType> GetFilteredAuxiliarAccessoryTypes(IQueryable<AuxiliarAccessoryType> query, AuxiliarAccessoryTypeSearchModel auxiliarAccessoryTypeSearch)
        {
            if (!string.IsNullOrEmpty(auxiliarAccessoryTypeSearch.Name))
            {
                query = query.Where(x => x.Name.Contains(auxiliarAccessoryTypeSearch.Name));
            }

            return query;
        }

        #endregion
    }
}
