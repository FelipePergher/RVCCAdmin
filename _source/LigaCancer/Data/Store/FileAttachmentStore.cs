using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
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
            TaskResult result = new TaskResult();
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
            TaskResult result = new TaskResult();
            try
            {
                //Todo remove file of disk too
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

        public Task<FileAttachment> FindByIdAsync(string id, ISpecification<FileAttachment> specification = null)
        {
            IQueryable<FileAttachment> query = _context.FileAttachments;

            if (specification != null)
            {
                if (specification.Includes.Any())
                {
                    query = specification.Includes.Aggregate(query, (current, inc) => current.Include(inc));
                }
                if (specification.IncludeStrings.Any())
                {
                    query = specification.IncludeStrings.Aggregate(query, (current, include) => current.Include(include));
                }
            }

            return Task.FromResult(query.FirstOrDefault(x => x.FileAttachmentId == int.Parse(id)));
        }

        public Task<List<FileAttachment>> GetAllAsync(string[] include = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<FileAttachment> query = _context.FileAttachments;

            if (include != null)
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(FileAttachment model)
        {
            TaskResult result = new TaskResult();
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
