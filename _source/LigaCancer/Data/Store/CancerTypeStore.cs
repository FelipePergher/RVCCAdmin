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
    public class CancerTypeStore : IDataStore<CancerType>, IDataTable<CancerType>
    {
        private readonly ApplicationDbContext _context;

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
                if (cancerType != null && cancerType.PatientInformationCancerTypes.Count > 0)
                {
                    result.Errors.Add(new TaskError
                    {
                        Code = "Acesso Negado",
                        Description = "Não é possível apagar este tipo de cancêr"
                    });
                    return Task.FromResult(result);
                }

                if (cancerType != null)
                {
                    _context.CancerTypes.Remove(cancerType);
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

        public Task<CancerType> FindByIdAsync(string id, ISpecification<CancerType> specification = null)
        {
            IQueryable<CancerType> queryable = _context.CancerTypes;

            if (specification != null)
            {
                queryable = queryable.IncludeExpressions(specification.Includes).IncludeByNames(specification.IncludeStrings);
            }

            return Task.FromResult(queryable.FirstOrDefault(x => x.CancerTypeId == int.Parse(id)));
        }

        public Task<List<CancerType>> GetAllAsync(string[] include = null, int take = int.MaxValue, int skip = 0)
        {
            IQueryable<CancerType> query = _context.CancerTypes;

            if (include != null)
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
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
            DataTableResponse data = await _context.Set<CancerType>()
                            .IncludeExpressions(spec.Includes)
                            .IncludeByNames(spec.IncludeStrings)
                            .GetOptionResponseAsync(options);

            return data;
        }

        #region Custom Methods

        public Task<CancerType> FindByNameAsync(string name, int CancerTypeId = -1)
        {
            return Task.FromResult(_context.CancerTypes.FirstOrDefault(x => x.Name == name && x.CancerTypeId != CancerTypeId));
        }

        #endregion
    }
}
