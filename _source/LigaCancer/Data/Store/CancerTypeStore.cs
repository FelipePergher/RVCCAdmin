using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Data.Requests;
using LigaCancer.Data.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Store
{
    public class CancerTypeStore : IDataStore<CancerType>, IDataTable<CancerType>
    {
        private ApplicationDbContext _context;

        public CancerTypeStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.CancerTypes.Count();
        }

        public Task<TaskResult> CreateAsync(CancerType model)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.CancerTypes.Add(model);
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

        public Task<TaskResult> DeleteAsync(CancerType model)
        {
            TaskResult result = new TaskResult();
            try
            {
                CancerType cancerType = _context.CancerTypes.Include(x => x.PatientInformationCancerTypes).FirstOrDefault(b => b.CancerTypeId == model.CancerTypeId);
                if (cancerType.PatientInformationCancerTypes.Count > 0)
                {
                    result.Errors.Add(new TaskError
                    {
                        Code = "Acesso Negado",
                        Description = "Não é possível apagar este tipo de cancêr"
                    });
                    return Task.FromResult(result);
                }
                cancerType.IsDeleted = true;
                cancerType.DeletedDate = DateTime.Now;
                cancerType.Name = DateTime.Now + "||" + cancerType.Name;
                _context.Update(cancerType);

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

        public Task<CancerType> FindByIdAsync(string id, string[] include = null)
        {
            IQueryable<CancerType> query = _context.CancerTypes;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.FirstOrDefault(x => x.CancerTypeId == int.Parse(id)));
        }

        public Task<List<CancerType>> GetAllAsync(string[] include = null)
        {
            IQueryable<CancerType> query = _context.CancerTypes;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(CancerType model)
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
        public async Task<DataTableResponse> GetOptionResponseWithSpec(DataTableOptions options, ISpecification<CancerType> spec)
        {
            var data = await _context.Set<CancerType>()
                            .IncludeExpressions(spec.Includes)
                            .IncludeByNames(spec.IncludeStrings)
                            .GetOptionResponseAsync(options);

            return data;
        }

        public async Task<DataTableResponse> GetOptionResponse(DataTableOptions options)
        {
            return await _context.Set<CancerType>().GetOptionResponseAsync(options);
        }

        #region Custom Methods

        public Task<CancerType> FindByNameAsync(string name, int CancerTypeId = -1)
        {
            if (CancerTypeId == -1)
            {
                CancerType cancerType = _context.CancerTypes.IgnoreQueryFilters().FirstOrDefault(x => x.Name == name);
                if (cancerType != null && cancerType.IsDeleted)
                {
                    cancerType.IsDeleted = false;
                    cancerType.LastUpdatedDate = DateTime.Now;
                }
                return Task.FromResult(cancerType);
            }

            return Task.FromResult(_context.CancerTypes.IgnoreQueryFilters().FirstOrDefault(x => x.Name == name && x.CancerTypeId != CancerTypeId));
        }

        #endregion
    }
}
