using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Models.SearchModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Store
{
    public class FileAttachmentStore : IDataStore<FileAttachment>
    {
        private readonly ApplicationDbContext _context;

        public FileAttachmentStore(ApplicationDbContext context)
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

        public void Dispose()
        {
            _context?.Dispose();
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

        public Task<List<FileAttachment>> GetAllAsync(string[] includes = null,
            string sortColumn = "", string sortDirection = "", object filter = null)
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

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(FileAttachment model)
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
