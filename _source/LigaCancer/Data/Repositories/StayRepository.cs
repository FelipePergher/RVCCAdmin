// <copyright file="StayRepository.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
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
    public class StayRepository : IDataRepository<Stay>
    {
        private readonly ApplicationDbContext _context;

        public StayRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Stays.Count();
        }

        public int CountByPatient(int patientId)
        {
            return _context.Stays.Count(x => x.PatientId == patientId);
        }

        public Task<TaskResult> CreateAsync(Stay stay)
        {
            var result = new TaskResult();
            try
            {
                _context.Stays.Add(stay);
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

        public Task<TaskResult> DeleteAsync(Stay stay)
        {
            var result = new TaskResult();
            try
            {
                _context.Stays.Remove(stay);
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

        public Task<Stay> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<Stay> query = _context.Stays;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.StayId == int.Parse(id)));
        }

        public Task<List<Stay>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Stay> query = _context.Stays;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationStays(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredStays(query, (StaySearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Stay stay)
        {
            var result = new TaskResult();
            try
            {
                stay.UpdatedTime = DateTime.Now;
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

        private IQueryable<Stay> GetOrdinationStays(IQueryable<Stay> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Date" => sortDirection == "asc"
                    ? query.OrderBy(x => x.StayDateTime.Date)
                    : query.OrderByDescending(x => x.StayDateTime.Date),
                "City" => sortDirection == "asc" ? query.OrderBy(x => x.City) : query.OrderByDescending(x => x.City),
                "Note" => sortDirection == "asc" ? query.OrderBy(x => x.Note) : query.OrderByDescending(x => x.Note),
                _ => sortDirection == "asc"
                    ? query.OrderBy(x => x.PatientName)
                    : query.OrderByDescending(x => x.PatientName)
            };
        }

        private IQueryable<Stay> GetFilteredStays(IQueryable<Stay> query, StaySearchModel staySearch)
        {
            if (!string.IsNullOrEmpty(staySearch.PatientId))
            {
                query = query.Where(x => x.PatientId == int.Parse(staySearch.PatientId));
            }

            if (!string.IsNullOrEmpty(staySearch.Name))
            {
                query = query.Where(x => x.PatientName.Contains(staySearch.Name));
            }

            if (!string.IsNullOrEmpty(staySearch.City))
            {
                query = query.Where(x => x.City.Contains(staySearch.City));
            }

            if (staySearch.DateFrom != null)
            {
                query = query.Where(x => x.StayDateTime.Date >= DateTime.Parse(staySearch.DateFrom).Date);
            }

            if (staySearch.DateTo != null)
            {
                query = query.Where(x => x.StayDateTime.Date <= DateTime.Parse(staySearch.DateTo).Date);
            }

            return query;
        }

        #endregion
    }
}
