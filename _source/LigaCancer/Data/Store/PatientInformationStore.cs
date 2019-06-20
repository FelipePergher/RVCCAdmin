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
    public class PatientInformationStore : IDataStore<PatientInformation>
    {
        private readonly ApplicationDbContext _context;

        public PatientInformationStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.PatientInformation.Count();
        }

        public Task<TaskResult> CreateAsync(PatientInformation patientInformation)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.PatientInformation.Add(patientInformation);
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

        public Task<TaskResult> DeleteAsync(PatientInformation patientInformation)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.PatientInformation.Remove(patientInformation);
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

        public Task<PatientInformation> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<PatientInformation> query = _context.PatientInformation;

            if (includes != null) query = includes.Aggregate(query, (current, inc) => current.Include(inc));

            return Task.FromResult(query.FirstOrDefault(x => x.PatientInformationId == int.Parse(id)));
        }

        public Task<List<PatientInformation>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<TaskResult> UpdateAsync(PatientInformation model)
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
