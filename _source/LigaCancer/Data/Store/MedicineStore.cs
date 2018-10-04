using LigaCancer.Code;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Store
{
    public class MedicineStore : IDataStore<Medicine>
    {
        private ApplicationDbContext _context;

        public MedicineStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Medicines.Count();
        }

        public Task<TaskResult> CreateAsync(Medicine model)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Medicines.Add(model);
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

        public Task<TaskResult> DeleteAsync(Medicine model)
        {
            TaskResult result = new TaskResult();
            try
            {
                Medicine medicine = _context.Medicines.FirstOrDefault(b => b.MedicineId == model.MedicineId);
                medicine.IsDeleted = true;
                medicine.DeletedDate = DateTime.Now;
                medicine.Name = DateTime.Now + "||" + medicine.Name;
                _context.Update(medicine);

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

        public Task<Medicine> FindByIdAsync(string id, string[] include = null)
        {
            IQueryable<Medicine> query = _context.Medicines;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.FirstOrDefault(x => x.MedicineId == int.Parse(id)));
        }

        public Task<List<Medicine>> GetAllAsync(string[] include = null)
        {
            IQueryable<Medicine> query = _context.Medicines;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Medicine model)
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

        public IQueryable<Medicine> GetAllQueryable(string[] include = null)
        {
            IQueryable<Medicine> query = _context.Medicines;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return query;
        }

        #region Custom Methods

        public Task<Medicine> FindByNameAsync(string name, int MedicineId)
        {
            Medicine medicine = _context.Medicines.IgnoreQueryFilters().FirstOrDefault(x => x.Name == name && x.MedicineId != MedicineId);
            return Task.FromResult(medicine);
        }

        #endregion
    }
}
