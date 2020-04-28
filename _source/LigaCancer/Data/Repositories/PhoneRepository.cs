using Microsoft.EntityFrameworkCore;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Data.Repositories
{
    public class PhoneRepository : IDataRepository<Phone>
    {
        private readonly ApplicationDbContext _context;

        public PhoneRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Phones.Count();
        }

        public Task<TaskResult> CreateAsync(Phone phone)
        {
            var result = new TaskResult();
            try
            {
                _context.Phones.Add(phone);
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

        public Task<TaskResult> DeleteAsync(Phone phone)
        {
            var result = new TaskResult();
            try
            {
                _context.Phones.Remove(phone);
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

        public Task<Phone> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<Phone> query = _context.Phones;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.PhoneId == int.Parse(id)));
        }

        public Task<List<Phone>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Phone> query = _context.Phones;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationPhones(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredPhones(query, (PhoneSearchModel)filter);
            }

            return Task.FromResult(query.ToList());

        }

        public Task<TaskResult> UpdateAsync(Phone model)
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

        private IQueryable<Phone> GetOrdinationPhones(IQueryable<Phone> query, string sortColumn, string sortDirection)
        {
            switch (sortColumn)
            {
                case "Number":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Number) : query.OrderByDescending(x => x.Number);
                case "PhoneType":
                    return sortDirection == "asc" ? query.OrderBy(x => x.PhoneType) : query.OrderByDescending(x => x.PhoneType);
                case "ObservationNote":
                    return sortDirection == "asc" ? query.OrderBy(x => x.ObservationNote) : query.OrderByDescending(x => x.ObservationNote);
                default:
                    return sortDirection == "asc" ? query.OrderBy(x => x.Number) : query.OrderByDescending(x => x.Number);
            }
        }

        private IQueryable<Phone> GetFilteredPhones(IQueryable<Phone> query, PhoneSearchModel phoneSearch)
        {
            if (!string.IsNullOrEmpty(phoneSearch.PatientId))
            {
                query = query.Where(x => x.PatientId == int.Parse(phoneSearch.PatientId));
            }

            return query;
        }

        #endregion
    }
}
