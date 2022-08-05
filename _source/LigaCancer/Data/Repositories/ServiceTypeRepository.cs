// <copyright file="ServiceTypeRepository.cs" company="Doffs">
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
    public class ServiceTypeRepository : IDataRepository<ServiceType>
    {
        private readonly ApplicationDbContext _context;

        public ServiceTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.ServiceTypes.Count();
        }

        public Task<TaskResult> CreateAsync(ServiceType serviceType)
        {
            var result = new TaskResult();
            try
            {
                _context.ServiceTypes.Add(serviceType);
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

        public Task<TaskResult> DeleteAsync(ServiceType serviceType)
        {
            var result = new TaskResult();
            try
            {
                _context.ServiceTypes.Remove(serviceType);
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

        public Task<ServiceType> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<ServiceType> query = _context.ServiceTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.ServiceTypeId == int.Parse(id)));
        }

        public Task<List<ServiceType>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<ServiceType> query = _context.ServiceTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationServiceType(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredServiceTypes(query, (ServiceTypeSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(ServiceType serviceType)
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

        public Task<ServiceType> FindByNameAsync(string name, int serviceTypeId = -1)
        {
            return Task.FromResult(_context.ServiceTypes.FirstOrDefault(x => x.Name == name && x.ServiceTypeId != serviceTypeId));
        }

        #endregion

        #region Private Methods

        private static IQueryable<ServiceType> GetOrdinationServiceType(IQueryable<ServiceType> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Quantity" => sortDirection == "asc" ? query.OrderBy(x => x.PatientInformationServiceTypes.Count) : query.OrderByDescending(x => x.PatientInformationServiceTypes.Count),
                _ => sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };
        }

        private static IQueryable<ServiceType> GetFilteredServiceTypes(IQueryable<ServiceType> query, ServiceTypeSearchModel serviceTypeSearch)
        {
            if (!string.IsNullOrEmpty(serviceTypeSearch.Name))
            {
                query = query.Where(x => x.Name.Contains(serviceTypeSearch.Name));
            }

            return query;
        }

        #endregion
    }
}
