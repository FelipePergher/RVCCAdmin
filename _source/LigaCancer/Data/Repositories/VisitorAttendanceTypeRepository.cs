// <copyright file="VisitorAttendanceTypeRepository.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.RelationModels;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Data.Repositories
{
    public class VisitorAttendanceTypeRepository : IDataRepository<VisitorAttendanceType>
    {
        private readonly ApplicationDbContext _context;

        public VisitorAttendanceTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.VisitorAttendanceTypes.Count();
        }

        public int CountByVisitor(int visitorId)
        {
            return _context.VisitorAttendanceTypes.Count(x => x.VisitorId == visitorId);
        }

        public Task<VisitorAttendanceType> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<VisitorAttendanceType> query = _context.VisitorAttendanceTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.VisitorAttendanceTypeId == int.Parse(id)));
        }

        public Task<TaskResult> CreateAsync(VisitorAttendanceType visitorAttendanceType)
        {
            var result = new TaskResult();
            try
            {
                _context.VisitorAttendanceTypes.Add(visitorAttendanceType);
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

        public Task<TaskResult> DeleteAsync(VisitorAttendanceType visitorAttendanceType)
        {
            var result = new TaskResult();
            try
            {
                _context.VisitorAttendanceTypes.Remove(visitorAttendanceType);
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

        public Task<List<VisitorAttendanceType>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<VisitorAttendanceType> query = _context.VisitorAttendanceTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationVisitorAttendanceTypes(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredVisitorAttendanceTypes(query, (VisitorAttendanceTypeSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(VisitorAttendanceType visitorAttendanceType)
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

        public List<int> GetMonthChartData(DateTime chartDate)
        {
            var data = new List<int>();

            var monthPresences = _context.VisitorAttendanceTypes
                .Where(x => x.AttendanceDate.Year == chartDate.Year && x.AttendanceDate.Month == chartDate.Month) as IEnumerable<VisitorAttendanceType>;

            int daysInMonth = DateTime.DaysInMonth(chartDate.Year, chartDate.Month);
            for (int i = 1; i <= daysInMonth; i++)
            {
                int dayCount = monthPresences.Count(x => x.AttendanceDate.Day == i);
                data.Add(dayCount);
            }

            return data;
        }

        public List<int> GetYearChartData(DateTime chartDate)
        {
            var data = new List<int>();

            var yearPresences = _context.VisitorAttendanceTypes.Where(x => x.AttendanceDate.Year == chartDate.Year) as IEnumerable<VisitorAttendanceType>;

            for (int i = 1; i < 13; i++)
            {
                int monthCount = yearPresences.Count(x => x.AttendanceDate.Month == i);
                data.Add(monthCount);
            }

            return data;
        }

        #endregion

        #region Private Methods

        private static IQueryable<VisitorAttendanceType> GetOrdinationVisitorAttendanceTypes(IQueryable<VisitorAttendanceType> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Visitor" => sortDirection == "asc"
                    ? query.OrderBy(x => x.Visitor.Name)
                    : query.OrderByDescending(x => x.Visitor.Name),
                "Attendant" => sortDirection == "asc"
                    ? query.OrderBy(x => x.Attendant.Name)
                    : query.OrderByDescending(x => x.Attendant.Name),
                "AttendanceType" => sortDirection == "asc"
                    ? query.OrderBy(x => x.AttendanceType.Name)
                    : query.OrderByDescending(x => x.AttendanceType.Name),
                "Date" => sortDirection == "asc"
                    ? query.OrderBy(x => x.AttendanceDate)
                    : query.OrderByDescending(x => x.AttendanceDate),
                "Observation" => sortDirection == "asc"
                    ? query.OrderBy(x => x.Observation)
                    : query.OrderByDescending(x => x.Observation),
                _ => sortDirection == "asc"
                    ? query.OrderBy(x => x.AttendanceType.Name)
                    : query.OrderByDescending(x => x.AttendanceType.Name),
            };
        }

        private static IQueryable<VisitorAttendanceType> GetFilteredVisitorAttendanceTypes(IQueryable<VisitorAttendanceType> query, VisitorAttendanceTypeSearchModel visitorAttendanceTypeSearch)
        {
            if (!string.IsNullOrEmpty(visitorAttendanceTypeSearch.Name))
            {
                query = query.Where(x => x.Visitor.Name.Contains(visitorAttendanceTypeSearch.Name));
            }

            if (!string.IsNullOrEmpty(visitorAttendanceTypeSearch.Attendant))
            {
                query = query.Where(x => x.Attendant.Name.Contains(visitorAttendanceTypeSearch.Attendant));
            }

            if (!string.IsNullOrEmpty(visitorAttendanceTypeSearch.Attendance))
            {
                query = query.Where(x => x.AttendanceType.Name.Contains(visitorAttendanceTypeSearch.Attendance));
            }

            if (!string.IsNullOrEmpty(visitorAttendanceTypeSearch.DateFrom))
            {
                query = query.Where(x => x.AttendanceDate.Date >= DateTime.Parse(visitorAttendanceTypeSearch.DateFrom).Date);
            }

            if (!string.IsNullOrEmpty(visitorAttendanceTypeSearch.DateTo))
            {
                query = query.Where(x => x.AttendanceDate.Date <= DateTime.Parse(visitorAttendanceTypeSearch.DateTo).Date);
            }

            return query;
        }

        #endregion
    }
}
