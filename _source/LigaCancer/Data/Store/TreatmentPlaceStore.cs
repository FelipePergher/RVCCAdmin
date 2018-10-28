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
    public class TreatmentPlaceStore : IDataStore<TreatmentPlace>, IDataTable<TreatmentPlace>
    {
        private ApplicationDbContext _context;

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
                if (treatmentPlace.PatientInformationTreatmentPlaces.Count > 0)
                {
                    result.Errors.Add(new TaskError
                    {
                        Code = "Acesso Negado",
                        Description = "Não é possível apagar esta cidade"
                    });
                    return Task.FromResult(result);
                }
                treatmentPlace.IsDeleted = true;
                treatmentPlace.DeletedDate = DateTime.Now;
                treatmentPlace.City = DateTime.Now + "||" + treatmentPlace.City;
                _context.Update(treatmentPlace);

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

        public Task<TreatmentPlace> FindByIdAsync(string id, string[] include = null)
        {
            IQueryable<TreatmentPlace> query = _context.TreatmentPlaces;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.FirstOrDefault(x => x.TreatmentPlaceId == int.Parse(id)));
        }

        public Task<List<TreatmentPlace>> GetAllAsync(string[] include = null)
        {
            IQueryable<TreatmentPlace> query = _context.TreatmentPlaces;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
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

        public IQueryable<TreatmentPlace> GetAllQueryable(string[] include = null)
        {
            IQueryable<TreatmentPlace> query = _context.TreatmentPlaces;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return query;
        }

        //IDataTable
        public async Task<DataTableResponse> GetOptionResponseWithSpec(DataTableOptions options, ISpecification<TreatmentPlace> spec)
        {
            var data = await _context.Set<TreatmentPlace>()
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

        public Task<TreatmentPlace> FindByCityAsync(string city, int TreatmentPlaceId = -1)
        {
            if(TreatmentPlaceId == -1)
            {
                TreatmentPlace treatmentPlace = _context.TreatmentPlaces.IgnoreQueryFilters().FirstOrDefault(x => x.City == city);
                if (treatmentPlace != null && treatmentPlace.IsDeleted)
                {
                    treatmentPlace.IsDeleted = false;
                    treatmentPlace.LastUpdatedDate = DateTime.Now;
                }
                return Task.FromResult(treatmentPlace);
            }

            return Task.FromResult(_context.TreatmentPlaces.IgnoreQueryFilters().FirstOrDefault(x => x.City == city && x.TreatmentPlaceId != TreatmentPlaceId));
        }

        #endregion
    }
}
