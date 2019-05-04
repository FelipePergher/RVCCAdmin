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
    public class FamilyMemberStore : IDataStore<FamilyMember>
    {
        private readonly ApplicationDbContext _context;

        public FamilyMemberStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.FamilyMembers.Count();
        }

        public Task<TaskResult> CreateAsync(FamilyMember familyMember)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.FamilyMembers.Add(familyMember);
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

        public Task<TaskResult> DeleteAsync(FamilyMember familyMember)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.FamilyMembers.Remove(familyMember);
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

        public Task<FamilyMember> FindByIdAsync(string id, ISpecification<FamilyMember> specification = null)
        {
            IQueryable<FamilyMember> query = _context.FamilyMembers;

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

            return Task.FromResult(query.FirstOrDefault(x => x.FamilyMemberId == int.Parse(id)));
        }

        public Task<List<FamilyMember>> GetAllAsync(string[] include = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<FamilyMember> query = _context.FamilyMembers;

            if (include != null)
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection)) query = GetOrdenationFamilyMembers(query, sortColumn, sortDirection);
            if (filter != null) query = GetFilteredFamilyMembers(query, (FamilyMemberSearchModel)filter);

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(FamilyMember model)
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

        #region Private Methods

        private IQueryable<FamilyMember> GetOrdenationFamilyMembers(IQueryable<FamilyMember> query, string sortColumn, string sortDirection)
        {
            switch (sortColumn)
            {
                case "Name":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                case "Kinship":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Kinship) : query.OrderByDescending(x => x.Kinship);
                case "Age":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Age) : query.OrderByDescending(x => x.Age);
                case "Sex":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Sex) : query.OrderByDescending(x => x.Sex);
                case "MonthlyIncome":
                    return sortDirection == "asc" ? query.OrderBy(x => x.MonthlyIncome) : query.OrderByDescending(x => x.MonthlyIncome);
                default:
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
        }

        private IQueryable<FamilyMember> GetFilteredFamilyMembers(IQueryable<FamilyMember> query, FamilyMemberSearchModel familyMemberSearch)
        {
            if (!string.IsNullOrEmpty(familyMemberSearch.PatientId)) query = query.Where(x => x.PatientId == int.Parse(familyMemberSearch.PatientId));
            return query;
        }

        #endregion

    }
}
