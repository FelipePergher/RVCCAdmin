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
        private ApplicationDbContext _context;

        public FileAttachmentStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.FileAttachments.Count();
        }

        public Task<TaskResult> CreateAsync(FileAttachment model)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.FileAttachments.Add(model);
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

        public Task<TaskResult> DeleteAsync(FileAttachment model)
        {
            TaskResult result = new TaskResult();
            try
            {
                FileAttachment fileAttachment = _context.FileAttachments.FirstOrDefault(b => b.FileAttachmentId == model.FileAttachmentId);
                fileAttachment.IsDeleted = true;
                fileAttachment.DeletedDate = DateTime.Now;
                _context.Update(fileAttachment);

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
            if(specification != null)
            {
                return Task.FromResult(
                _context.FileAttachments
                .IncludeExpressions(specification.Includes)
                .IncludeByNames(specification.IncludeStrings)
                .FirstOrDefault(x => x.FileAttachmentId == int.Parse(id)));
            }
            else
            {
                return Task.FromResult(_context.FileAttachments.FirstOrDefault(x => x.FileAttachmentId == int.Parse(id)));
            }
            
        }

        public Task<List<FileAttachment>> GetAllAsync(string[] include = null)
        {
            IQueryable<FileAttachment> query = _context.FileAttachments;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
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
