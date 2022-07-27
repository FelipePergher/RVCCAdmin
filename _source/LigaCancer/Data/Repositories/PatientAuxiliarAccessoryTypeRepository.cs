// <copyright file="PatientAuxiliarAccessoryTypeRepository.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models.RelationModels;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Data.Repositories
{
    public class PatientAuxiliarAccessoryTypeRepository : IDataRepository<PatientAuxiliarAccessoryType>
    {
        private readonly ApplicationDbContext _context;

        public PatientAuxiliarAccessoryTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.PatientAuxiliarAccessoryTypes.Count();
        }

        public int CountByPatient(int patientId)
        {
            return _context.PatientAuxiliarAccessoryTypes.Count(x => x.PatientId == patientId);
        }

        public Task<PatientAuxiliarAccessoryType> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<PatientAuxiliarAccessoryType> query = _context.PatientAuxiliarAccessoryTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.PatientAuxiliarAccessoryTypeId == int.Parse(id)));
        }

        public Task<TaskResult> CreateAsync(PatientAuxiliarAccessoryType patientAuxiliarAccessoryType)
        {
            var result = new TaskResult();
            try
            {
                _context.PatientAuxiliarAccessoryTypes.Add(patientAuxiliarAccessoryType);
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

        public Task<TaskResult> DeleteAsync(PatientAuxiliarAccessoryType patientAuxiliarAccessoryType)
        {
            var result = new TaskResult();
            try
            {
                _context.PatientAuxiliarAccessoryTypes.Remove(patientAuxiliarAccessoryType);
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

        public Task<List<PatientAuxiliarAccessoryType>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<PatientAuxiliarAccessoryType> query = _context.PatientAuxiliarAccessoryTypes;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationPatientAuxiliarAccessoryTypes(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredPatientAuxiliarAccessoryTypes(query, (PatientAuxiliarAccessoryTypeSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(PatientAuxiliarAccessoryType patientAuxiliarAccessoryType)
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

        private static IQueryable<PatientAuxiliarAccessoryType> GetOrdinationPatientAuxiliarAccessoryTypes(IQueryable<PatientAuxiliarAccessoryType> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "AuxiliarAccessoryTypeTime" => sortDirection == "asc"
                    ? query.OrderBy(x => x.AuxiliarAccessoryTypeTime)
                    : query.OrderByDescending(x => x.AuxiliarAccessoryTypeTime),
                "Note" => sortDirection == "asc"
                    ? query.OrderBy(x => x.Note)
                    : query.OrderByDescending(x => x.Note),
                "DuoDate" => sortDirection == "asc"
                    ? query.OrderBy(x => x.DuoDate)
                    : query.OrderByDescending(x => x.DuoDate),
                _ => sortDirection == "asc"
                    ? query.OrderBy(x => x.AuxiliarAccessoryType.Name)
                    : query.OrderByDescending(x => x.AuxiliarAccessoryType.Name),
            };
        }

        private static IQueryable<PatientAuxiliarAccessoryType> GetFilteredPatientAuxiliarAccessoryTypes(IQueryable<PatientAuxiliarAccessoryType> query, PatientAuxiliarAccessoryTypeSearchModel patientAuxiliarAccessoryTypeSearch)
        {
            if (!string.IsNullOrEmpty(patientAuxiliarAccessoryTypeSearch.PatientId))
            {
                query = query.Where(x => x.PatientId == int.Parse(patientAuxiliarAccessoryTypeSearch.PatientId));
            }

            return query;
        }

        #endregion
    }
}
