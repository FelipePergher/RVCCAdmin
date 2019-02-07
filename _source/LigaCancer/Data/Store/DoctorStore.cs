﻿using LigaCancer.Code;
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
    public class DoctorStore : IDataStore<Doctor>
    {
        private readonly ApplicationDbContext _context;

        public DoctorStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Doctors.Count();
        }

        public Task<TaskResult> CreateAsync(Doctor doctor)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Doctors.Add(doctor);
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

        public Task<TaskResult> DeleteAsync(Doctor doctor)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Doctors.Remove(doctor);
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

        public Task<Doctor> FindByIdAsync(string id, ISpecification<Doctor> specification = null)
        {
            IQueryable<Doctor> queryable = _context.Doctors;

            if (specification != null)
            {
                queryable = queryable.IncludeExpressions(specification.Includes).IncludeByNames(specification.IncludeStrings);
            }

            return Task.FromResult(queryable.FirstOrDefault(x => x.DoctorId == int.Parse(id)));
        }

        public Task<List<Doctor>> GetAllAsync(string[] include = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Doctor> query = _context.Doctors;

            if (include != null)
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection)) query = GetOrdenationDoctor(query, sortColumn, sortDirection);
            if (filter != null) query = GetFilteredDoctors(query, (DoctorSearchModel)filter);

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Doctor model)
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

        public Task<Doctor> FindByCrmAsync(string crm, int doctorId)
        {
            Doctor doctor = _context.Doctors.FirstOrDefault(x => x.CRM == crm && x.DoctorId != doctorId);
            return Task.FromResult(doctor);
        }

        public Task<Doctor> FindByNameAsync(string name)
        {
            Doctor doctor = _context.Doctors.FirstOrDefault(x => x.Name == name);
            return Task.FromResult(doctor);
        }

        #endregion

        #region Private Methods

        private IQueryable<Doctor> GetOrdenationDoctor(IQueryable<Doctor> query, string sortColumn, string sortDirection)
        {
            switch (sortColumn)
            {
                case "name":
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
                case "crm":
                    return sortDirection == "asc" ? query.OrderBy(x => x.CRM) : query.OrderByDescending(x => x.CRM);
                default:
                    return sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name);
            }
        }

        private IQueryable<Doctor> GetFilteredDoctors(IQueryable<Doctor> query, DoctorSearchModel doctorSearch)
        {
            if (!string.IsNullOrEmpty(doctorSearch.Name)) query = query.Where(x => x.Name.Contains(doctorSearch.Name));
            if (!string.IsNullOrEmpty(doctorSearch.CRM)) query = query.Where(x => x.CRM.Contains(doctorSearch.CRM));
            return query;
        }

        #endregion
    }
}
