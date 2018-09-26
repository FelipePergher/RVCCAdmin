using LigaCancer.Code;
using LigaCancer.Data.Models.Patient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Store
{
    public class DoctorStore : IDataStore<Doctor>
    {
        private ApplicationDbContext _context;

        public DoctorStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public Task<TaskResult> CreateAsync(Doctor model)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Doctors.Add(model);
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

        public Task<TaskResult> DeleteAsync(Doctor model)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public Task<Doctor> FindByIdAsync(string id, string[] include = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<Doctor>> GetAllAsync(string[] include = null)
        {
            IQueryable<Doctor> query = _context.Doctors;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Doctor model)
        {
            throw new NotImplementedException();
        }
    }
}
