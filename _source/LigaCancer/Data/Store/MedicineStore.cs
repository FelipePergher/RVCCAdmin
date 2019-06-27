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
    public class MedicineStore : IDataStore<Medicine>
    {
        private readonly ApplicationDbContext _context;

        public MedicineStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Medicines.Count();
        }

        public Task<TaskResult> CreateAsync(Medicine medicine)
        {
            var result = new TaskResult();
            try
            {
                _context.Medicines.Add(medicine);
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

        public Task<TaskResult> DeleteAsync(Medicine medicine)
        {
            var result = new TaskResult();
            try
            {
                _context.Medicines.Remove(medicine);
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

        public Task<Medicine> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<Medicine> query = _context.Medicines;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.MedicineId == int.Parse(id)));
        }

        public Task<List<Medicine>> GetAllAsync(string[] includes = null,
            string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Medicine> query = _context.Medicines;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdenationMedicine(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredMedicines(query, (MedicineSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Medicine model)
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

        #region Custom Methods

        public Task<Medicine> FindByNameAsync(string name, int MedicineId = -1)
        {
            return Task.FromResult(_context.Medicines.FirstOrDefault(x => x.Name == name && x.MedicineId != MedicineId));
        }

        #endregion

        #region Private Methods

        private IQueryable<Medicine> GetOrdenationMedicine(IQueryable<Medicine> query, string sortColumn, string sortDirection)
        {
            switch (sortColumn)
            {
                case "Name":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                default:
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
        }

        private IQueryable<Medicine> GetFilteredMedicines(IQueryable<Medicine> query, MedicineSearchModel doctorSearch)
        {
            if (!string.IsNullOrEmpty(doctorSearch.Name))
            {
                query = query.Where(x => x.Name.Contains(doctorSearch.Name));
            }

            return query;
        }

        #endregion
    }
}
