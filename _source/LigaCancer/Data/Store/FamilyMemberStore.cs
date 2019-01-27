﻿using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
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

        public Task<TaskResult> CreateAsync(FamilyMember model)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.FamilyMembers.Add(model);
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

        public Task<TaskResult> DeleteAsync(FamilyMember model)
        {
            TaskResult result = new TaskResult();
            try
            {
                FamilyMember familyMember = _context.FamilyMembers.FirstOrDefault(b => b.FamilyMemberId == model.FamilyMemberId);
                Family family = _context.Families.Include(x => x.FamilyMembers).FirstOrDefault(x => x.FamilyMembers.FirstOrDefault(y => y.FamilyMemberId == model.FamilyMemberId) != null);
                if (family != null)
                {
                    family.FamilyIncome -= (double)familyMember.MonthlyIncome;
                    family.PerCapitaIncome = family.FamilyIncome / (family.FamilyMembers.Count());
                }

                if (familyMember != null)
                {
                    _context.FamilyMembers.Remove(familyMember);
                }

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
            IQueryable<FamilyMember> queryable = _context.FamilyMembers;
            
            if (specification != null)
            {
                queryable = queryable.IncludeExpressions(specification.Includes).IncludeByNames(specification.IncludeStrings);
            }

            return Task.FromResult(queryable.FirstOrDefault(x => x.FamilyMemberId == int.Parse(id)));
        }

        public Task<List<FamilyMember>> GetAllAsync(string[] include = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<FamilyMember> query = _context.FamilyMembers;

            if (include != null)
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(FamilyMember model)
        {
            TaskResult result = new TaskResult();
            try
            {
                Family family = _context.Families.Include(x => x.FamilyMembers).FirstOrDefault(x => x.FamilyMembers.FirstOrDefault(y => y.FamilyMemberId == model.FamilyMemberId) != null);
                if(family != null)
                {
                    family.FamilyIncome += (double)model.MonthlyIncome;
                    family.PerCapitaIncome = family.FamilyIncome / (family.FamilyMembers.Count() + 1);
                }
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

        public Task<TaskResult> UpdateFamilyIncomeByFamilyMember(FamilyMember familyMember)
        {
            TaskResult result = new TaskResult();
            try
            {
                Family family = _context.Families.FirstOrDefault(x => x.FamilyMembers.FirstOrDefault(y => y.FamilyMemberId == familyMember.FamilyMemberId) != null);
                if (family != null)
                {
                    family.FamilyIncome -= (double)familyMember.MonthlyIncome;
                }
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

        #endregion

    }
}
