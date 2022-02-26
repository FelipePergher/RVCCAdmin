// <copyright file="AdminInfoRepository.cs" company="Doffs">
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
    public class AdminInfoRepository : IDataRepository<AdminInfo>
    {
        private readonly ApplicationDbContext _context;

        public AdminInfoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.AdminInfos.Count();
        }

        public Task<TaskResult> CreateAsync(AdminInfo adminInfo)
        {
            var result = new TaskResult();
            try
            {
                _context.AdminInfos.Add(adminInfo);
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

        public Task<TaskResult> DeleteAsync(AdminInfo adminInfo)
        {
            var result = new TaskResult();
            try
            {
                _context.AdminInfos.Remove(adminInfo);
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

        public Task<AdminInfo> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<AdminInfo> query = _context.AdminInfos;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.AdminInfoId == int.Parse(id)));
        }

        public Task<List<AdminInfo>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<AdminInfo> query = _context.AdminInfos;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(AdminInfo adminInfo)
        {
            var result = new TaskResult();
            try
            {
                adminInfo.UpdatedTime = DateTime.Now;
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
