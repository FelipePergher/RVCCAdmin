using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Code.Requests;
using LigaCancer.Code.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Store
{
    public class MedicineStore : IDataStore<Medicine>, IDataTable<Medicine>
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
                Medicine medicine = _context.Medicines.Include(x => x.PatientInformationMedicines).FirstOrDefault(b => b.MedicineId == model.MedicineId);
                if (medicine != null && medicine.PatientInformationMedicines.Count > 0)
                {
                    result.Errors.Add(new TaskError
                    {
                        Code = "Acesso Negado",
                        Description = "Não é possível apagar este remédio"
                    });
                    return Task.FromResult(result);
                }

                if (medicine != null)
                {
                    _context.Medicines.Remove(medicine);
                }

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

        public Task<Medicine> FindByIdAsync(string id, ISpecification<Medicine> specification = null)
        {
            IQueryable<Medicine> queryable = _context.Medicines;

            if (specification != null)
            {
                queryable = queryable.IncludeExpressions(specification.Includes).IncludeByNames(specification.IncludeStrings);
            }

            return Task.FromResult(queryable.FirstOrDefault(x => x.MedicineId == int.Parse(id)));
        }

        public Task<List<Medicine>> GetAllAsync(string[] include = null, int take = int.MaxValue, int skip = 0)
        {
            IQueryable<Medicine> query = _context.Medicines;

            if (include != null)
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
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

        //IDataTable
        public async Task<DataTableResponse> GetOptionResponseWithSpec(DataTableOptions options, ISpecification<Medicine> spec)
        {
            DataTableResponse data = await _context.Set<Medicine>()
                            .IncludeExpressions(spec.Includes)
                            .IncludeByNames(spec.IncludeStrings)
                            .GetOptionResponseAsync(options);

            return data;
        }

        #region Custom Methods

        public Task<Medicine> FindByNameAsync(string name, int MedicineId = -1)
        {
            return Task.FromResult(_context.Medicines.FirstOrDefault(x => x.Name == name && x.MedicineId != MedicineId));
        }

        #endregion
    }
}
