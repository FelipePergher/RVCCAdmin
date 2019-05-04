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
    public class AddressStore : IDataStore<Address>
    {
        private readonly ApplicationDbContext _context;

        public AddressStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Addresses.Count();
        }

        public Task<TaskResult> CreateAsync(Address address)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Addresses.Add(address);
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

        public Task<TaskResult> DeleteAsync(Address address)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Addresses.Remove(address);
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

        public Task<Address> FindByIdAsync(string id, ISpecification<Address> specification = null)
        {
            IQueryable<Address> query = _context.Addresses;

            if (specification != null)
            {
                if (specification.Includes.Any())
                {
                    query = specification.Includes.Aggregate(query, (current, inc) => current.Include(inc));
                }
                if (specification.IncludeStrings.Any())
                {
                    query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
                }
            }

            return Task.FromResult(query.FirstOrDefault(x => x.AddressId == int.Parse(id)));
        }

        public Task<List<Address>> GetAllAsync(string[] include = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Address> query = _context.Addresses;

            if (include != null)
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection)) query = GetOrdenationAddresses(query, sortColumn, sortDirection);
            if (filter != null) query = GetFilteredAddresses(query, (AddressSearchModel)filter);

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Address model)
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

        #region Private Methods

        private IQueryable<Address> GetOrdenationAddresses(IQueryable<Address> query, string sortColumn, string sortDirection)
        {
            switch (sortColumn)
            {
                case "Street":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Street) : query.OrderByDescending(x => x.Street);
                case "Neighborhood":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Neighborhood) : query.OrderByDescending(x => x.Neighborhood);
                case "City":
                    return sortDirection == "asc" ? query.OrderBy(x => x.City) : query.OrderByDescending(x => x.City);
                case "HouseNumber":
                    return sortDirection == "asc" ? query.OrderBy(x => x.HouseNumber) : query.OrderByDescending(x => x.HouseNumber);
                case "Complement":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Complement) : query.OrderByDescending(x => x.Complement);
                case "ResidenceType":
                    return sortDirection == "asc" ? query.OrderBy(x => x.ResidenceType) : query.OrderByDescending(x => x.ResidenceType);
                case "MonthlyAmmountResidence":
                    return sortDirection == "asc" ? query.OrderBy(x => x.MonthlyAmmountResidence) : query.OrderByDescending(x => x.MonthlyAmmountResidence);
                case "ObservationAddress":
                    return sortDirection == "asc" ? query.OrderBy(x => x.ObservationAddress) : query.OrderByDescending(x => x.ObservationAddress);
                default:
                    return sortDirection == "asc" ? query.OrderBy(x => x.Street) : query.OrderByDescending(x => x.Street);
            }
        }

        private IQueryable<Address> GetFilteredAddresses(IQueryable<Address> query, AddressSearchModel addressSearch)
        {
            if (!string.IsNullOrEmpty(addressSearch.PatientId)) query = query.Where(x => x.PatientId == int.Parse(addressSearch.PatientId));
            return query;
        }

        #endregion
    }
}
