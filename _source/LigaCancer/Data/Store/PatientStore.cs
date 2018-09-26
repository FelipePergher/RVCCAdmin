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

        #region Default Methods

        public int Count()
        {
            return 1;
        }

        public Task<Patient> CreateAsync(Patient model)
        {
            throw new NotImplementedException();
        }

        public Task<Patient> DeleteAsync(Patient model)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            _context?.Dispose();
        }

        public Task<Patient> FindByIdAsync(string id, string[] include = null)
        {
            IQueryable<Patient> query = _context.Patients;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.FirstOrDefault(b => b.PatientId.ToString() == id));
        }

        public Task<List<Patient>> GetAllAsync(string[] include = null)
        {
            throw new NotImplementedException();
        }

        public Task<Patient> UpdateAsync(Patient model)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
