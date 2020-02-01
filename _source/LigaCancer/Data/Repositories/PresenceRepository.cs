﻿using Microsoft.EntityFrameworkCore;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.PatientModels;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Data.Repositories
{
    public class PresenceRepository : IDataRepository<Presence>
    {
        private readonly ApplicationDbContext _context;

        public PresenceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Presences.Count();
        }

        public Task<TaskResult> CreateAsync(Presence presence)
        {
            var result = new TaskResult();
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
            var result = new TaskResult();
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

        public Task<Presence> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<Presence> query = _context.Presences;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.PresenceId == int.Parse(id)));
        }

        public Task<List<Presence>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Presence> query = _context.Presences;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdenationPresences(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredPresences(query, (PresenceSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Presence model)
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

        public List<int> GetDayChartData(DateTime chartDate)
        {
            var data = new List<int>();

            var todayPresences = _context.Presences
                .Where(x => x.PresenceDateTime.Year == chartDate.Year && x.PresenceDateTime.Month == chartDate.Month && x.PresenceDateTime.Day == chartDate.Day) as IEnumerable<Presence>;

            for (int i = 0; i < 24; i++)
            {
                int hourCount = todayPresences.Count(x => x.PresenceDateTime.TimeOfDay.Hours == i);
                data.Add(hourCount);
            }

            return data;
        }

        public List<int> GetMonthChartData(DateTime chartDate)
        {
            var data = new List<int>();

            var monthPresences = _context.Presences
                .Where(x => x.PresenceDateTime.Year == chartDate.Year && x.PresenceDateTime.Month == chartDate.Month) as IEnumerable<Presence>;

            int daysInMonth = DateTime.DaysInMonth(chartDate.Year, chartDate.Month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                int dayCount = monthPresences.Count(x => x.PresenceDateTime.Day == i);
                data.Add(dayCount);
            }

            return data;
        }

        public List<int> GetYearChartData(DateTime chartDate)
        {
            var data = new List<int>();

            var yearPresences = _context.Presences.Where(x => x.PresenceDateTime.Year == chartDate.Year) as IEnumerable<Presence>;

            for (int i = 1; i < 13; i++)
            {
                int monthCount = yearPresences.Count(x => x.PresenceDateTime.Month == i);
                data.Add(monthCount);
            }

            return data;
        }

        #endregion

        #region Private Methods

        private IQueryable<Presence> GetOrdenationPresences(IQueryable<Presence> query, string sortColumn, string sortDirection)
        {
            switch (sortColumn)
            {
                case "Patient":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                case "Date":
                    return sortDirection == "asc" ? query.OrderBy(x => x.PresenceDateTime.Date) : query.OrderByDescending(x => x.PresenceDateTime.Date);
                case "Hour":
                    return sortDirection == "asc" ? query.OrderBy(x => $"{x.PresenceDateTime.Hour}{x.PresenceDateTime.Minute}") : query.OrderByDescending(x => $"{x.PresenceDateTime.Hour}{x.PresenceDateTime.Minute}");
                default:
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
        }

        private IQueryable<Presence> GetFilteredPresences(IQueryable<Presence> query, PresenceSearchModel presenceSearch)
        {
            if (!string.IsNullOrEmpty(presenceSearch.Name))
            {
                query = query.Where(x => x.Name.Contains(presenceSearch.Name));
            }

            if (presenceSearch.DateFrom != null)
            {
                query = query.Where(x => x.PresenceDateTime.Date >= DateTime.Parse(presenceSearch.DateFrom).Date);
            }

            if (presenceSearch.DateTo != null)
            {
                query = query.Where(x => x.PresenceDateTime.Date <= DateTime.Parse(presenceSearch.DateTo).Date);
            }

            return query;
        }

        #endregion
    }
}