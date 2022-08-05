// <copyright file="AttendanceTypeRepository.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Data.Repositories
{
    public class AttendanceTypeRepository : IDataRepository<AttendanceType>
    {
        private readonly ApplicationDbContext _context;

        public AttendanceTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.AttendanceTypes.Count();
        }

        public Task<TaskResult> CreateAsync(AttendanceType attendanceType)
        {
            var result = new TaskResult();
            try
            {
                _context.AttendanceTypes.Add(attendanceType);
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

        public Task<TaskResult> DeleteAsync(AttendanceType attendanceType)
        {
            var result = new TaskResult();
            try
            {
                _context.AttendanceTypes.Remove(attendanceType);
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

        public Task<AttendanceType> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<AttendanceType> query = _context.AttendanceTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.AttendanceTypeId == int.Parse(id)));
        }

        public Task<List<AttendanceType>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<AttendanceType> query = _context.AttendanceTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationAttendanceType(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredAttendanceTypes(query, (AttendanceTypeSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(AttendanceType attendanceType)
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

        public Task<AttendanceType> FindByNameAsync(string name, int attendanceTypeId = -1)
        {
            return Task.FromResult(_context.AttendanceTypes.FirstOrDefault(x => x.Name == name && x.AttendanceTypeId != attendanceTypeId));
        }

        #endregion

        #region Private Methods

        private static IQueryable<AttendanceType> GetOrdinationAttendanceType(IQueryable<AttendanceType> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Quantity" => sortDirection == "asc" ? query.OrderBy(x => x.VisitorAttendanceTypes.Count) : query.OrderByDescending(x => x.VisitorAttendanceTypes.Count),
                _ => sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };
        }

        private static IQueryable<AttendanceType> GetFilteredAttendanceTypes(IQueryable<AttendanceType> query, AttendanceTypeSearchModel attendanceTypeSearch)
        {
            if (!string.IsNullOrEmpty(attendanceTypeSearch.Name))
            {
                query = query.Where(x => x.Name.Contains(attendanceTypeSearch.Name));
            }

            return query;
        }

        #endregion
    }
}
