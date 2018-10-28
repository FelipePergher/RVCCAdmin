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
        private ApplicationDbContext _context;

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
                Phone phone = _context.Phones.FirstOrDefault(b => b.PhoneId == model.PhoneId);
                phone.IsDeleted = true;
                phone.DeletedDate = DateTime.Now;
                _context.Update(phone);

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

        public Task<Phone> FindByIdAsync(string id, string[] include = null)
        {
            IQueryable<Phone> query = _context.Phones;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.FirstOrDefault(x => x.PhoneId == int.Parse(id)));
        }

        public Task<List<Phone>> GetAllAsync(string[] include = null)
        {
            IQueryable<Phone> query = _context.Phones;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
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

        public IQueryable<Phone> GetAllQueryable(string[] include = null)
        {
            IQueryable<Phone> query = _context.Phones;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return query;
        }

    }
}
