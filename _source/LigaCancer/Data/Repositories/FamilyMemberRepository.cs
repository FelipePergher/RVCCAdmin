// <copyright file="FamilyMemberRepository.cs" company="Doffs">
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
    public class FamilyMemberRepository : IDataRepository<FamilyMember>
    {
        private readonly ApplicationDbContext _context;

        public FamilyMemberRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.FamilyMembers.Count();
        }

        public Task<TaskResult> CreateAsync(FamilyMember familyMember)
        {
            var result = new TaskResult();
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
            var result = new TaskResult();
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

        public Task<FamilyMember> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<FamilyMember> query = _context.FamilyMembers;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.FamilyMemberId == int.Parse(id)));
        }

        public Task<List<FamilyMember>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<FamilyMember> query = _context.FamilyMembers;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationFamilyMembers(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredFamilyMembers(query, (FamilyMemberSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(FamilyMember familyMember)
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

        private static IQueryable<FamilyMember> GetOrdinationFamilyMembers(IQueryable<FamilyMember> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Kinship" => sortDirection == "asc"
                    ? query.OrderBy(x => x.Kinship)
                    : query.OrderByDescending(x => x.Kinship),
                "DateOfBirth" => sortDirection == "asc"
                    ? query.OrderBy(x => x.DateOfBirth)
                    : query.OrderByDescending(x => x.DateOfBirth),
                "Sex" => sortDirection == "asc" ? query.OrderBy(x => x.Sex) : query.OrderByDescending(x => x.Sex),
                "MonthlyIncome" => sortDirection == "asc"
                    ? query.OrderBy(x => x.MonthlyIncomeMinSalary)
                    : query.OrderByDescending(x => x.MonthlyIncomeMinSalary),
                _ => sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };
        }

        private static IQueryable<FamilyMember> GetFilteredFamilyMembers(IQueryable<FamilyMember> query, FamilyMemberSearchModel familyMemberSearch)
        {
            if (!string.IsNullOrEmpty(familyMemberSearch.PatientId))
            {
                query = query.Where(x => x.PatientId == int.Parse(familyMemberSearch.PatientId));
            }

            return query;
        }

        #endregion

    }
}
