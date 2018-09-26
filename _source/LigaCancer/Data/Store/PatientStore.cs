using LigaCancer.Code;
using LigaCancer.Data.Models.Patient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Store
{
    public class PatientStore : IDataStore<Patient>
    {
        private ApplicationDbContext _context;

        public PatientStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            throw new NotImplementedException();
        }

        public Task<TaskResult> CreateAsync(Patient model)
        {
            throw new NotImplementedException();
        }

        public Task<TaskResult> DeleteAsync(Patient model)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<Patient> FindByIdAsync(string id, string[] include = null)
        {
            throw new NotImplementedException();
        }

        public Task<List<Patient>> GetAllAsync(string[] include = null)
        {
            throw new NotImplementedException();
        }

        public Task<TaskResult> UpdateAsync(Patient model)
        {
            throw new NotImplementedException();
        }
    }
}
