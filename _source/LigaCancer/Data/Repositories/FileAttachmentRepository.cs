// <copyright file="FileAttachmentRepository.cs" company="Doffs">
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
    public class FileAttachmentRepository : IDataRepository<FileAttachment>
    {
        private readonly ApplicationDbContext _context;

        public FileAttachmentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.FileAttachments.Count();
        }

        public Task<TaskResult> CreateAsync(FileAttachment fileAttachment)
        {
            var result = new TaskResult();
            try
            {
                _context.FileAttachments.Add(fileAttachment);
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

        public Task<TaskResult> DeleteAsync(FileAttachment fileAttachment)
        {
            var result = new TaskResult();
            try
            {
                _context.FileAttachments.Remove(fileAttachment);
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

        public Task<FileAttachment> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<FileAttachment> query = _context.FileAttachments;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.FileAttachmentId == int.Parse(id)));
        }

        public Task<List<FileAttachment>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<FileAttachment> query = _context.FileAttachments;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            var fileAttachmentSearch = (FileAttachmentSearchModel)filter;
            if (!string.IsNullOrEmpty(fileAttachmentSearch.PatientId))
            {
                query = query.Where(x => x.PatientId == int.Parse(fileAttachmentSearch.PatientId));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationFiles(query, sortColumn, sortDirection);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(FileAttachment fileAttachment)
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

        private static IQueryable<FileAttachment> GetOrdinationFiles(IQueryable<FileAttachment> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Name" => sortDirection == "asc"
                    ? query.OrderBy(x => x.FileName)
                    : query.OrderByDescending(x => x.FileName),
                _ => sortDirection == "asc"
                    ? query.OrderBy(x => x.FileSize)
                    : query.OrderByDescending(x => x.FileSize)
            };
        }

        #endregion

    }
}
