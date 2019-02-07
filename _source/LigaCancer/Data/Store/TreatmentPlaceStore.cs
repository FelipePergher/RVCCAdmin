using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Store
{
    public class TreatmentPlaceStore : IDataStore<TreatmentPlace>
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

        public Task<TaskResult> CreateAsync(TreatmentPlace treatmentPlace)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.TreatmentPlaces.Add(treatmentPlace);
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

        public Task<TaskResult> DeleteAsync(TreatmentPlace treatmentPlace)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.TreatmentPlaces.Remove(treatmentPlace);
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

        public Task<TreatmentPlace> FindByIdAsync(string id, ISpecification<TreatmentPlace> specification = null)
        {
            IQueryable<TreatmentPlace> queryable = _context.TreatmentPlaces;

            if (specification != null)
            {
                queryable = queryable.IncludeExpressions(specification.Includes).IncludeByNames(specification.IncludeStrings);
            }

            return Task.FromResult(queryable.FirstOrDefault(x => x.TreatmentPlaceId == int.Parse(id)));
        }

        public Task<List<TreatmentPlace>> GetAllAsync(string[] include = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<TreatmentPlace> query = _context.TreatmentPlaces;

            if (include != null)
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(TreatmentPlace treatmentPlace)
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

        #region Custom Methods

        public Task<TreatmentPlace> FindByCityAsync(string city, int treatmentPlaceId = 1)
        {
            return Task.FromResult(_context.TreatmentPlaces.FirstOrDefault(x => x.City == city && x.TreatmentPlaceId != treatmentPlaceId));
        }

        #endregion
    }
}
