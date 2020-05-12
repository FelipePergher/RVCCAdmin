// <copyright file="PresenceRepository.cs" company="Felipe Pergher">
// Copyright (c) Felipe Pergher. All Rights Reserved.
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

        public int CountByPatient(int patientId)
        {
            return _context.Presences.Count(x => x.PatientId == patientId);
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
                });
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
                query = GetOrdinationPresences(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredPresences(query, (PresenceSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Presence presence)
        {
            var result = new TaskResult();
            try
            {
                presence.UpdatedTime = DateTime.Now;
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

        private IQueryable<Presence> GetOrdinationPresences(IQueryable<Presence> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Date" => sortDirection == "asc"
                    ? query.OrderBy(x => x.PresenceDateTime.Date)
                    : query.OrderByDescending(x => x.PresenceDateTime.Date),
                "Hour" => sortDirection == "asc"
                    ? query.OrderBy(x => $"{x.PresenceDateTime.Hour}{x.PresenceDateTime.Minute}")
                    : query.OrderByDescending(x => $"{x.PresenceDateTime.Hour}{x.PresenceDateTime.Minute}"),
                _ => sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };
        }

        private IQueryable<Presence> GetFilteredPresences(IQueryable<Presence> query, PresenceSearchModel presenceSearch)
        {
            if (!string.IsNullOrEmpty(presenceSearch.PatientId))
            {
                query = query.Where(x => x.PatientId == int.Parse(presenceSearch.PatientId));
            }

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
