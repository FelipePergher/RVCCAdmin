// <copyright file="BenefitRepository.cs" company="Felipe Pergher">
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
    public class BenefitRepository : IDataRepository<Benefit>
    {
        private readonly ApplicationDbContext _context;

        public BenefitRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Benefits.Count();
        }

        public Task<TaskResult> CreateAsync(Benefit benefit)
        {
            var result = new TaskResult();
            try
            {
                _context.Benefits.Add(benefit);
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

        public Task<TaskResult> DeleteAsync(Benefit benefit)
        {
            var result = new TaskResult();
            try
            {
                _context.Benefits.Remove(benefit);
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

        public Task<Benefit> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<Benefit> query = _context.Benefits;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.BenefitId == int.Parse(id)));
        }

        public Task<List<Benefit>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Benefit> query = _context.Benefits;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationBenefit(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredBenefits(query, (BenefitSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Benefit benefit)
        {
            var result = new TaskResult();
            try
            {
                benefit.UpdatedTime = DateTime.Now;
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

        public Task<Benefit> FindByNameAsync(string name, int benefitId = -1)
        {
            return Task.FromResult(_context.Benefits.FirstOrDefault(x => x.Name == name && x.BenefitId != benefitId));
        }

        #endregion

        #region Private Methods

        private IQueryable<Benefit> GetOrdinationBenefit(IQueryable<Benefit> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Note" => sortDirection == "asc" ? query.OrderBy(x => x.Note) : query.OrderByDescending(x => x.Note),
                _ => sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };
        }

        private IQueryable<Benefit> GetFilteredBenefits(IQueryable<Benefit> query, BenefitSearchModel benefitSearch)
        {
            if (!string.IsNullOrEmpty(benefitSearch.Name))
            {
                query = query.Where(x => x.Name.Contains(benefitSearch.Name));
            }

            return query;
        }

        #endregion
    }
}
