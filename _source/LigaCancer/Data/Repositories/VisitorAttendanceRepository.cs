// <copyright file="VisitorAttendanceRepository.cs" company="Doffs">
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
    public class VisitorAttendanceRepository : IDataRepository<VisitorAttendance>
    {
        private readonly ApplicationDbContext _context;

        public VisitorAttendanceRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.VisitorAttendances.Count();
        }

        public int CountByVisitor(int visitorId)
        {
            return _context.VisitorAttendances.Count(x => x.VisitorId == visitorId);
        }

        public Task<VisitorAttendance> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<VisitorAttendance> query = _context.VisitorAttendances;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.VisitorAttendanceTypeId == int.Parse(id)));
        }

        public Task<TaskResult> CreateAsync(VisitorAttendance visitorAttendance)
        {
            var result = new TaskResult();
            try
            {
                _context.VisitorAttendances.Add(visitorAttendance);
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

        public Task<TaskResult> DeleteAsync(VisitorAttendance visitorAttendance)
        {
            var result = new TaskResult();
            try
            {
                _context.VisitorAttendances.Remove(visitorAttendance);
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

        public Task<List<VisitorAttendance>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<VisitorAttendance> query = _context.VisitorAttendances;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationVisitorAttendances(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredVisitorAttendances(query, (VisitorAttendanceSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(VisitorAttendance visitorAttendance)
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

        #region Private Methods

        private static IQueryable<VisitorAttendance> GetOrdinationVisitorAttendances(IQueryable<VisitorAttendance> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Attendance" => sortDirection == "asc"
                    ? query.OrderBy(x => x.Visitor.Name)
                    : query.OrderByDescending(x => x.Visitor.Name),
                "Date" => sortDirection == "asc"
                    ? query.OrderBy(x => x.AttendanceDate)
                    : query.OrderByDescending(x => x.AttendanceDate),
                "Observation" => sortDirection == "asc"
                    ? query.OrderBy(x => x.Observation)
                    : query.OrderByDescending(x => x.Observation),
                _ => sortDirection == "asc"
                    ? query.OrderBy(x => x.AttendanceType.Name)
                    : query.OrderByDescending(x => x.AttendanceType.Name)
            };
        }

        private static IQueryable<VisitorAttendance> GetFilteredVisitorAttendances(IQueryable<VisitorAttendance> query, VisitorAttendanceSearchModel visitorAttendanceSearch)
        {
            if (!string.IsNullOrEmpty(visitorAttendanceSearch.VisitorId))
            {
                query = query.Where(x => x.VisitorId == int.Parse(visitorAttendanceSearch.VisitorId));
            }

            if (!string.IsNullOrEmpty(visitorAttendanceSearch.Name))
            {
                query = query.Where(x => x.Visitor.Name.Contains(visitorAttendanceSearch.Name));
            }

            if (!string.IsNullOrEmpty(visitorAttendanceSearch.Attendance))
            {
                query = query.Where(x => x.AttendanceType.Name.Contains(visitorAttendanceSearch.Attendance));
            }

            if (visitorAttendanceSearch.DateFrom != null && string.IsNullOrEmpty(visitorAttendanceSearch.VisitorId))
            {
                query = query.Where(x => x.AttendanceDate.Date >= DateTime.Parse(visitorAttendanceSearch.DateFrom).Date);
            }

            if (visitorAttendanceSearch.DateTo != null && string.IsNullOrEmpty(visitorAttendanceSearch.VisitorId))
            {
                query = query.Where(x => x.AttendanceDate.Date <= DateTime.Parse(visitorAttendanceSearch.DateTo).Date);
            }

            return query;
        }

        #endregion
    }
}
