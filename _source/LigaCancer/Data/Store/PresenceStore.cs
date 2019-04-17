using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using LigaCancer.Models.SearchModel;

namespace LigaCancer.Data.Store
{
    public class PresenceStore : IDataStore<Presence>
    {
        private readonly ApplicationDbContext _context;

        public PresenceStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Presences.Count();
        }

        public Task<TaskResult> CreateAsync(Presence presence)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Presences.Add(presence);
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

        public Task<TaskResult> DeleteAsync(Presence presence)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Presences.Remove(presence);
                _context.SaveChanges();
                result.Succeeded = true;
            }
            catch (Exception e)
            {
                result.Errors.Add(new TaskError
                {
                    Code = e.HResult.ToString(),
                    Description = e.Message
                }); ;
            }
           
            return Task.FromResult(result);
        }

        public void Dispose()
        {
           _context?.Dispose();
        }

        public Task<Presence> FindByIdAsync(string id, ISpecification<Presence> specification = null)
        {
            IQueryable<Presence> query = _context.Presences;

            if (specification != null)
            {
                if (specification.Includes.Any())
                {
                    query = specification.Includes.Aggregate(query, (current, inc) => current.Include(inc));
                }
                if (specification.IncludeStrings.Any())
                {
                    query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
                }
            }

            return Task.FromResult(query.FirstOrDefault(x => x.PresenceId == int.Parse(id)));
        }

        public Task<List<Presence>> GetAllAsync(string[] include = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Presence> query = _context.Presences;

            if (include != null)
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection)) query = GetOrdenationPresences(query, sortColumn, sortDirection);
            if (filter != null) query = GetFilteredPresences(query, (PresenceSearchModel) filter);

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Presence model)
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

        #region Private Methods

        private IQueryable<Presence> GetOrdenationPresences(IQueryable<Presence> query, string sortColumn, string sortDirection)
        {
            switch (sortColumn)
            {
                case "patient":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name); 
                case "date":
                    return sortDirection == "asc" ? query.OrderBy(x => x.PresenceDateTime.Date) : query.OrderByDescending(x => x.PresenceDateTime.Date);
                case "hour":
                    return sortDirection == "asc" ? query.OrderBy(x => $"{x.PresenceDateTime.Hour}{x.PresenceDateTime.Minute}") : query.OrderByDescending(x => $"{x.PresenceDateTime.Hour}{x.PresenceDateTime.Minute}");
                default:
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
        }

        private IQueryable<Presence> GetFilteredPresences(IQueryable<Presence> query, PresenceSearchModel presenceSearch)
        {
            if (!string.IsNullOrEmpty(presenceSearch.Name)) query = query.Where(x => x.Name.Contains(presenceSearch.Name));
            if (!string.IsNullOrEmpty(presenceSearch.Surname)) query = query.Where(x => x.Name.Contains(presenceSearch.Surname));
            
            if (presenceSearch.DateFrom != null) query = query.Where(x => x.PresenceDateTime.Date >= presenceSearch.DateFrom.Value.Date);
            if (presenceSearch.DateTo != null) query = query.Where(x => x.PresenceDateTime.Date <= presenceSearch.DateTo.Value.Date);

            return query;
        }

        #endregion
    }
}
