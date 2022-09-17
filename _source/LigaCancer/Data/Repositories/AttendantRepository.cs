// <copyright file="AttendantRepository.cs" company="Doffs">
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
    public class AttendantRepository : IDataRepository<Attendant>
    {
        private readonly ApplicationDbContext _context;

        public AttendantRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Attendants.Count();
        }

        public Task<TaskResult> CreateAsync(Attendant attendant)
        {
            var result = new TaskResult();
            try
            {
                _context.Attendants.Add(attendant);
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

        public Task<TaskResult> DeleteAsync(Attendant attendant)
        {
            var result = new TaskResult();
            try
            {
                _context.Attendants.Remove(attendant);
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

        public Task<Attendant> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<Attendant> query = _context.Attendants;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.AttendantId == int.Parse(id)));
        }

        public Task<List<Attendant>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Attendant> query = _context.Attendants;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationAttendant(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredAttendants(query, (AttendantSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Attendant attendant)
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

        public Task<Attendant> FindByNameAsync(string name, int attendantId = -1)
        {
            return Task.FromResult(_context.Attendants.FirstOrDefault(x => x.Name == name && x.AttendantId != attendantId));
        }

        #endregion

        #region Private Methods

        private static IQueryable<Attendant> GetOrdinationAttendant(IQueryable<Attendant> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "CPF" => sortDirection == "asc" ? query.OrderBy(x => x.CPF) : query.OrderByDescending(x => x.CPF),
                "Phone" => sortDirection == "asc" ? query.OrderBy(x => x.Phone) : query.OrderByDescending(x => x.Phone),
                "Note" => sortDirection == "asc" ? query.OrderBy(x => x.Note) : query.OrderByDescending(x => x.Note),
                "Quantity" => sortDirection == "asc" ? query.OrderBy(x => x.VisitorAttendanceTypes.Count) : query.OrderByDescending(x => x.VisitorAttendanceTypes.Count),
                _ => sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };
        }

        private static IQueryable<Attendant> GetFilteredAttendants(IQueryable<Attendant> query, AttendantSearchModel attendantSearch)
        {
            if (!string.IsNullOrEmpty(attendantSearch.Name))
            {
                query = query.Where(x => x.Name.Contains(attendantSearch.Name));
            }

            if (!string.IsNullOrEmpty(attendantSearch.CPF))
            {
                query = query.Where(x => x.CPF.Contains(attendantSearch.CPF));
            }

            if (!string.IsNullOrEmpty(attendantSearch.Phone))
            {
                query = query.Where(x => x.Phone.Contains(attendantSearch.Phone));
            }

            if (!string.IsNullOrEmpty(attendantSearch.Note))
            {
                query = query.Where(x => x.Note.Contains(attendantSearch.Note));
            }

            return query;
        }

        #endregion
    }
}
