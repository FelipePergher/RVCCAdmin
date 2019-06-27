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
    public class PhoneStore : IDataStore<Phone>
    {
        private readonly ApplicationDbContext _context;

        public PhoneStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Phones.Count();
        }

        public Task<TaskResult> CreateAsync(Phone phone)
        {
            TaskResult result = new TaskResult();
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
            TaskResult result = new TaskResult();
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

            if (includes != null) query = includes.Aggregate(query, (current, inc) => current.Include(inc));

            return Task.FromResult(query.FirstOrDefault(x => x.PhoneId == int.Parse(id)));
        }

        public Task<List<Phone>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Phone> query = _context.Phones;

            if (includes != null) query = includes.Aggregate(query, (current, inc) => current.Include(inc));

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection)) query = GetOrdenationPhones(query, sortColumn, sortDirection);
            if (filter != null) query = GetFilteredPhones(query, (PhoneSearchModel)filter);

            return Task.FromResult(query.ToList());

        }

        public Task<TaskResult> UpdateAsync(Phone model)
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

        private IQueryable<Phone> GetOrdenationPhones(IQueryable<Phone> query, string sortColumn, string sortDirection)
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
            if (!string.IsNullOrEmpty(phoneSearch.PatientId)) query = query.Where(x => x.PatientId == int.Parse(phoneSearch.PatientId));
            return query;
        }

        #endregion
    }
}
