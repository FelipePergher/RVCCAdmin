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
    public class TreatmentPlaceRepository : IDataRepository<TreatmentPlace>
    {
        private readonly ApplicationDbContext _context;

        public TreatmentPlaceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.TreatmentPlaces.Count();
        }

        public Task<TaskResult> CreateAsync(TreatmentPlace treatmentPlace)
        {
            var result = new TaskResult();
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
            var result = new TaskResult();
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

        public Task<TreatmentPlace> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<TreatmentPlace> query = _context.TreatmentPlaces;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.TreatmentPlaceId == int.Parse(id)));
        }

        public Task<List<TreatmentPlace>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<TreatmentPlace> query = _context.TreatmentPlaces;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationTreatmentPlace(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredTreatmentPlaces(query, (TreatmentPlaceSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(TreatmentPlace treatmentPlace)
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

        public Task<TreatmentPlace> FindByCityAsync(string city, int treatmentPlaceId = 1)
        {
            return Task.FromResult(_context.TreatmentPlaces.FirstOrDefault(x => x.City == city && x.TreatmentPlaceId != treatmentPlaceId));
        }

        #endregion

        #region Private Methods

        private IQueryable<TreatmentPlace> GetOrdinationTreatmentPlace(IQueryable<TreatmentPlace> query, string sortColumn, string sortDirection)
        {
            switch (sortColumn)
            {
                case "City":
                    return sortDirection == "asc" ? query.OrderBy(x => x.City) : query.OrderByDescending(x => x.City);
                default:
                    return sortDirection == "asc" ? query.OrderBy(x => x.City) : query.OrderByDescending(x => x.City);
            }
        }

        private IQueryable<TreatmentPlace> GetFilteredTreatmentPlaces(IQueryable<TreatmentPlace> query, TreatmentPlaceSearchModel treatmentPlaceSearch)
        {
            if (!string.IsNullOrEmpty(treatmentPlaceSearch.City))
            {
                query = query.Where(x => x.City.Contains(treatmentPlaceSearch.City));
            }

            return query;
        }

        #endregion
    }
}
