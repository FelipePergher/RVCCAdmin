﻿using LigaCancer.Code;
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
    public class CancerTypeStore : IDataStore<CancerType>
    {
        private readonly ApplicationDbContext _context;

        public CancerTypeStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.CancerTypes.Count();
        }

        public Task<TaskResult> CreateAsync(CancerType cancerType)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.CancerTypes.Add(cancerType);
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

        public Task<TaskResult> DeleteAsync(CancerType cancerType)
        {
            TaskResult result = new TaskResult();
            try
            {
                
                _context.CancerTypes.Remove(cancerType);
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

        public Task<CancerType> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<CancerType> query = _context.CancerTypes;

            if (includes != null) query = includes.Aggregate(query, (current, inc) => current.Include(inc));

            return Task.FromResult(query.FirstOrDefault(x => x.CancerTypeId == int.Parse(id)));
        }

        public Task<List<CancerType>> GetAllAsync(string[] includes = null,
            string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<CancerType> query = _context.CancerTypes;

            if (includes != null) query = includes.Aggregate(query, (current, inc) => current.Include(inc));

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection)) query = GetOrdenationCancerType(query, sortColumn, sortDirection);
            if (filter != null) query = GetFilteredCancerTypes(query, (CancerTypeSearchModel)filter);

            return Task.FromResult(query.ToList());

        }

        public Task<TaskResult> UpdateAsync(CancerType model)
        {
            TaskResult result = new TaskResult();
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

        public Task<CancerType> FindByNameAsync(string name, int CancerTypeId = -1)
        {
            return Task.FromResult(_context.CancerTypes.FirstOrDefault(x => x.Name == name && x.CancerTypeId != CancerTypeId));
        }

        #endregion

        #region Private Methods

        private IQueryable<CancerType> GetOrdenationCancerType(IQueryable<CancerType> query, string sortColumn, string sortDirection)
        {
            switch (sortColumn)
            {
                case "Name":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                default:
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
        }

        private IQueryable<CancerType> GetFilteredCancerTypes(IQueryable<CancerType> query, CancerTypeSearchModel CancerTypeSearch)
        {
            if (!string.IsNullOrEmpty(CancerTypeSearch.Name)) query = query.Where(x => x.Name.Contains(CancerTypeSearch.Name));
            return query;
        }

        #endregion
    }
}