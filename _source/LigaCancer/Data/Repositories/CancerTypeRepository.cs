﻿// <copyright file="CancerTypeRepository.cs" company="Doffs">
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
    public class CancerTypeRepository : IDataRepository<CancerType>
    {
        private readonly ApplicationDbContext _context;

        public CancerTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.CancerTypes.Count();
        }

        public Task<TaskResult> CreateAsync(CancerType cancerType)
        {
            var result = new TaskResult();
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
            var result = new TaskResult();
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

        public Task<CancerType> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<CancerType> query = _context.CancerTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.CancerTypeId == int.Parse(id)));
        }

        public Task<List<CancerType>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<CancerType> query = _context.CancerTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationCancerType(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredCancerTypes(query, (CancerTypeSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(CancerType cancerType)
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

        public Task<CancerType> FindByNameAsync(string name, int cancerTypeId = -1)
        {
            return Task.FromResult(_context.CancerTypes.FirstOrDefault(x => x.Name == name && x.CancerTypeId != cancerTypeId));
        }

        #endregion

        #region Private Methods

        private static IQueryable<CancerType> GetOrdinationCancerType(IQueryable<CancerType> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Quantity" => sortDirection == "asc" ? query.OrderBy(x => x.PatientInformationCancerTypes.Count) : query.OrderByDescending(x => x.PatientInformationCancerTypes.Count),
                _ => sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };
        }

        private static IQueryable<CancerType> GetFilteredCancerTypes(IQueryable<CancerType> query, CancerTypeSearchModel cancerTypeSearch)
        {
            if (!string.IsNullOrEmpty(cancerTypeSearch.Name))
            {
                query = query.Where(x => x.Name.Contains(cancerTypeSearch.Name));
            }

            return query;
        }

        #endregion
    }
}
