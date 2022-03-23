// <copyright file="NaturalityRepository.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Data.Repositories
{
    public class NaturalityRepository : IDataRepository<Naturality>
    {
        private readonly ApplicationDbContext _context;

        public NaturalityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Naturalities.Count();
        }

        public Task<TaskResult> CreateAsync(Naturality naturality)
        {
            var result = new TaskResult();
            try
            {
                _context.Naturalities.Add(naturality);
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

        public Task<TaskResult> DeleteAsync(Naturality naturality)
        {
            var result = new TaskResult();
            try
            {
                _context.Naturalities.Remove(naturality);
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

        public Task<Naturality> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<Naturality> query = _context.Naturalities;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.NaturalityId == int.Parse(id)));
        }

        public Task<List<Naturality>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<TaskResult> UpdateAsync(Naturality naturality)
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
    }
}
