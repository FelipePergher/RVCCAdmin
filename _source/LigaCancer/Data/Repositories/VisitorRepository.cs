// <copyright file="VisitorRepository.cs" company="Doffs">
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
    public class VisitorRepository : IDataRepository<Visitor>
    {
        private readonly ApplicationDbContext _context;

        public VisitorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Visitors.Count();
        }

        public Task<TaskResult> CreateAsync(Visitor visitor)
        {
            var result = new TaskResult();
            try
            {
                _context.Visitors.Add(visitor);
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

        public Task<TaskResult> DeleteAsync(Visitor visitor)
        {
            var result = new TaskResult();
            try
            {
                _context.Visitors.Remove(visitor);
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

        public Task<Visitor> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<Visitor> query = _context.Visitors;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.VisitorId == int.Parse(id)));
        }

        public Task<List<Visitor>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Visitor> query = _context.Visitors;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationVisitor(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredVisitors(query, (VisitorSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Visitor visitor)
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

        public Task<Visitor> FindByNameAsync(string name, int visitorId = -1)
        {
            return Task.FromResult(_context.Visitors.FirstOrDefault(x => x.Name == name && x.VisitorId != visitorId));
        }

        #endregion

        #region Private Methods

        private static IQueryable<Visitor> GetOrdinationVisitor(IQueryable<Visitor> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "CPF" => sortDirection == "asc" ? query.OrderBy(x => x.CPF) : query.OrderByDescending(x => x.CPF),
                "Phone" => sortDirection == "asc" ? query.OrderBy(x => x.Phone) : query.OrderByDescending(x => x.Phone),
                "Quantity" => sortDirection == "asc" ? query.OrderBy(x => x.VisitorAttendanceTypes.Count) : query.OrderByDescending(x => x.VisitorAttendanceTypes.Count),
                _ => sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };
        }

        private static IQueryable<Visitor> GetFilteredVisitors(IQueryable<Visitor> query, VisitorSearchModel visitorSearch)
        {
            if (!string.IsNullOrEmpty(visitorSearch.Name))
            {
                query = query.Where(x => x.Name.Contains(visitorSearch.Name));
            }

            if (!string.IsNullOrEmpty(visitorSearch.CPF))
            {
                query = query.Where(x => x.CPF.Contains(visitorSearch.CPF));
            }

            if (!string.IsNullOrEmpty(visitorSearch.Phone))
            {
                query = query.Where(x => x.Phone.Contains(visitorSearch.Phone));
            }

            return query;
        }

        #endregion
    }
}
