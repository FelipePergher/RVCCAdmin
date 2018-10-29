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
    public class ProfessionStore : IDataStore<Profession>
    {
        private ApplicationDbContext _context;

        public ProfessionStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Professions.Count();
        }

        public Task<TaskResult> CreateAsync(Profession model)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Professions.Add(model);
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

        public Task<TaskResult> DeleteAsync(Profession model)
        {
            TaskResult result = new TaskResult();
            try
            {
                Profession profession = _context.Professions.FirstOrDefault(b => b.ProfessionId == model.ProfessionId);
                profession.IsDeleted = true;
                profession.DeletedDate = DateTime.Now;
                profession.Name = DateTime.Now + "||" + profession.Name;
                _context.Update(profession);

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

        public Task<Profession> FindByIdAsync(string id, ISpecification<Profession> specification = null)
        {
            return Task.FromResult(
                _context.Professions
                .IncludeExpressions(specification.Includes)
                .IncludeByNames(specification.IncludeStrings)
                .FirstOrDefault(x => x.ProfessionId == int.Parse(id)));
        }

        public Task<List<Profession>> GetAllAsync(string[] include = null)
        {
            IQueryable<Profession> query = _context.Professions;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Profession model)
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

        #region Custom Methods

        public Task<Profession> FindByNameAsync(string name)
        {
            return Task.FromResult(_context.Professions.FirstOrDefault(x => x.Name == name));
        }

        #endregion

    }
}
