using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
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

        public Task<TaskResult> CreateAsync(Phone model)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Phones.Add(model);
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

        public Task<TaskResult> DeleteAsync(Phone model)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Phones.Remove(model);
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

        public Task<Phone> FindByIdAsync(string id, ISpecification<Phone> specification = null)
        {
            IQueryable<Phone> queryable = _context.Phones;

            if (specification != null)
            {
                queryable = queryable.IncludeExpressions(specification.Includes).IncludeByNames(specification.IncludeStrings);
            }

            return Task.FromResult(queryable.FirstOrDefault(x => x.PhoneId == int.Parse(id)));
        }

        public Task<List<Phone>> GetAllAsync(string[] include = null)
        {
            IQueryable<Phone> query = _context.Phones;

            if (include != null)
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
            }

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

    }
}
