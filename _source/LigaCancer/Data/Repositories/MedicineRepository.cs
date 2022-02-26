// <copyright file="MedicineRepository.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Data.Repositories
{
    public class MedicineRepository : IDataRepository<Medicine>
    {
        private readonly ApplicationDbContext _context;

        public MedicineRepository(ApplicationDbContext context)
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

        public Task<Medicine> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<Medicine> query = _context.Medicines;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.MedicineId == int.Parse(id)));
        }

        public Task<List<Medicine>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Medicine> query = _context.Medicines;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationMedicine(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredMedicines(query, (MedicineSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Medicine medicine)
        {
            var result = new TaskResult();
            try
            {
                medicine.UpdatedTime = DateTime.Now;
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

        public Task<Medicine> FindByNameAsync(string name, int medicineId = -1)
        {
            return Task.FromResult(_context.Medicines.FirstOrDefault(x => x.Name == name && x.MedicineId != medicineId));
        }

        #endregion

        #region Private Methods

        private IQueryable<Medicine> GetOrdinationMedicine(IQueryable<Medicine> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                _ => sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };
        }

        private IQueryable<Medicine> GetFilteredMedicines(IQueryable<Medicine> query, MedicineSearchModel medicineSearch)
        {
            if (!string.IsNullOrEmpty(medicineSearch.Name))
            {
                query = query.Where(x => x.Name.Contains(medicineSearch.Name));
            }

            return query;
        }

        #endregion
    }
}
