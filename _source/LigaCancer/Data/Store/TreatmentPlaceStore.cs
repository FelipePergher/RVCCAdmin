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
    public class TreatmentPlaceStore : IDataStore<TreatmentPlace>, IDataTable<TreatmentPlace>
    {
        private readonly ApplicationDbContext _context;

        public TreatmentPlaceStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.TreatmentPlaces.Count();
        }

        public Task<TaskResult> CreateAsync(TreatmentPlace model)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.TreatmentPlaces.Add(model);
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

        public Task<TaskResult> DeleteAsync(TreatmentPlace model)
        {
            TaskResult result = new TaskResult();
            try
            {
                TreatmentPlace treatmentPlace = _context.TreatmentPlaces.Include(x => x.PatientInformationTreatmentPlaces).FirstOrDefault(b => b.TreatmentPlaceId == model.TreatmentPlaceId);
                if (treatmentPlace != null && treatmentPlace.PatientInformationTreatmentPlaces.Count > 0)
                {
                    result.Errors.Add(new TaskError
                    {
                        Code = "Acesso Negado",
                        Description = "Não é possível apagar esta cidade"
                    });
                    return Task.FromResult(result);
                }

                if (treatmentPlace != null)
                {
                    treatmentPlace.IsDeleted = true;
                    treatmentPlace.DeletedDate = DateTime.Now;
                    treatmentPlace.City = DateTime.Now + "||" + treatmentPlace.City;
                    _context.Update(treatmentPlace);
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

        public Task<TreatmentPlace> FindByIdAsync(string id, ISpecification<TreatmentPlace> specification = null, bool ignoreQueryFilter = false)
        {
            IQueryable<TreatmentPlace> queryable = _context.TreatmentPlaces;
            if (ignoreQueryFilter)
            {
                queryable = queryable.IgnoreQueryFilters();
            }

            if (specification != null)
            {
                queryable = queryable.IncludeExpressions(specification.Includes).IncludeByNames(specification.IncludeStrings);
            }

            return Task.FromResult(queryable.FirstOrDefault(x => x.TreatmentPlaceId == int.Parse(id)));
        }

        public Task<List<TreatmentPlace>> GetAllAsync(string[] include = null)
        {
            IQueryable<TreatmentPlace> query = _context.TreatmentPlaces;

            if (include != null)
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(TreatmentPlace model)
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
        public async Task<DataTableResponse> GetOptionResponseWithSpec(DataTableOptions options, ISpecification<TreatmentPlace> spec)
        {
            DataTableResponse data = await _context.Set<TreatmentPlace>()
                            .IncludeExpressions(spec.Includes)
                            .IncludeByNames(spec.IncludeStrings)
                            .GetOptionResponseAsync(options);

            return data;
        }

        public async Task<DataTableResponse> GetOptionResponse(DataTableOptions options)
        {
            return await _context.Set<TreatmentPlace>().GetOptionResponseAsync(options);
        }

        #region Custom Methods

        public Task<TreatmentPlace> FindByCityAsync(string city, int treatmentPlaceId = -1)
        {
            if (treatmentPlaceId != -1) return Task.FromResult(_context.TreatmentPlaces.IgnoreQueryFilters()
                    .FirstOrDefault(x => x.City == city && x.TreatmentPlaceId != treatmentPlaceId));

            TreatmentPlace treatmentPlace = _context.TreatmentPlaces.IgnoreQueryFilters().FirstOrDefault(x => x.City == city);
            if (treatmentPlace == null || !treatmentPlace.IsDeleted) return Task.FromResult(treatmentPlace);

            treatmentPlace.IsDeleted = false;
            treatmentPlace.LastUpdatedDate = DateTime.Now;
            return Task.FromResult(treatmentPlace);
        }

        #endregion
    }
}
