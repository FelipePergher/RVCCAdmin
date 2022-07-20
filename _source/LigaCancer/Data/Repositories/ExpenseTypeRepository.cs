// <copyright file="ExpenseTypeRepository.cs" company="Doffs">
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
    public class ExpenseTypeRepository : IDataRepository<ExpenseType>
    {
        private readonly ApplicationDbContext _context;

        public ExpenseTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.ExpenseTypes.Count();
        }

        public Task<TaskResult> CreateAsync(ExpenseType expenseType)
        {
            var result = new TaskResult();
            try
            {
                _context.ExpenseTypes.Add(expenseType);
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

        public Task<TaskResult> DeleteAsync(ExpenseType expenseType)
        {
            var result = new TaskResult();
            try
            {
                _context.ExpenseTypes.Remove(expenseType);
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

        public Task<ExpenseType> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<ExpenseType> query = _context.ExpenseTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.ExpenseTypeId == int.Parse(id)));
        }

        public Task<List<ExpenseType>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<ExpenseType> query = _context.ExpenseTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationExpenseType(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredExpenseTypes(query, (ExpenseTypeSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(ExpenseType expenseType)
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

        public Task<ExpenseType> FindByNameAsync(string name, int expenseTypeId = -1)
        {
            return Task.FromResult(_context.ExpenseTypes.FirstOrDefault(x => x.Name == name && x.ExpenseTypeId != expenseTypeId));
        }

        #endregion

        #region Private Methods

        private static IQueryable<ExpenseType> GetOrdinationExpenseType(IQueryable<ExpenseType> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Quantity" => sortDirection == "asc" ? query.OrderBy(x => x.PatientExpenseTypes.Count) : query.OrderByDescending(x => x.PatientExpenseTypes.Count),
                "ExpenseTypeFrequency" => sortDirection == "asc" ? query.OrderBy(x => x.ExpenseTypeFrequency) : query.OrderByDescending(x => x.ExpenseTypeFrequency),
                _ => sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };
        }

        private static IQueryable<ExpenseType> GetFilteredExpenseTypes(IQueryable<ExpenseType> query, ExpenseTypeSearchModel expenseTypeSearch)
        {
            if (!string.IsNullOrEmpty(expenseTypeSearch.Name))
            {
                query = query.Where(x => x.Name.Contains(expenseTypeSearch.Name));
            }

            return query;
        }

        #endregion
    }
}
