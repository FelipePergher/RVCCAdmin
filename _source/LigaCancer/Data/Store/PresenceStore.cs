using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace LigaCancer.Data.Store
{
    public class PresenceStore : IDataStore<Presence>
    {
        private readonly ApplicationDbContext _context;

        public PresenceStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Presences.Count();
        }

        public Task<TaskResult> CreateAsync(Presence model)
        {
            TaskResult result = new TaskResult();
            _context.Presences.Add(model);
            _context.SaveChanges();
            result.Succeeded = true;
            return Task.FromResult(result);

        }

        public Task<TaskResult> DeleteAsync(Presence model)
        {
            TaskResult result = new TaskResult();
            _context.Presences.Remove(model);
            _context.SaveChanges();
            result.Succeeded = true;
            return Task.FromResult(result);
        }

        public void Dispose()
        {
           _context?.Dispose();
        }

        public Task<Presence> FindByIdAsync(string id, ISpecification<Presence> specification = null, bool ignoreQueryFilter = false)
        {
            return Task.FromResult(_context.Presences.Include(x => x.Patient).FirstOrDefault());
        }

        public Task<List<Presence>> GetAllAsync(string[] include = null)
        {
            return Task.FromResult(_context.Presences.Include(x => x.Patient).ToList());
        }

        public Task<TaskResult> UpdateAsync(Presence model)
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
