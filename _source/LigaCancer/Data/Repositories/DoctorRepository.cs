// <copyright file="DoctorRepository.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Data.Repositories
{
    public class DoctorRepository : IDataRepository<Doctor>
    {
        private readonly ApplicationDbContext _context;

        public DoctorRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Doctors.Count();
        }

        public Task<TaskResult> CreateAsync(Doctor doctor)
        {
            var result = new TaskResult();
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
            var result = new TaskResult();
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

        public Task<Doctor> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<Doctor> query = _context.Doctors;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.DoctorId == int.Parse(id)));
        }

        public Task<List<Doctor>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<Doctor> query = _context.Doctors;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationDoctor(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredDoctors(query, (DoctorSearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Doctor doctor)
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

        private static IQueryable<Doctor> GetOrdinationDoctor(IQueryable<Doctor> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "CRM" => sortDirection == "asc" ? query.OrderBy(x => x.CRM) : query.OrderByDescending(x => x.CRM),
                _ => sortDirection == "asc" ? query.OrderBy(x => x.Name) : query.OrderByDescending(x => x.Name)
            };
        }

        private static IQueryable<Doctor> GetFilteredDoctors(IQueryable<Doctor> query, DoctorSearchModel doctorSearch)
        {
            if (!string.IsNullOrEmpty(doctorSearch.Name))
            {
                query = query.Where(x => x.Name.Contains(doctorSearch.Name));
            }

            if (!string.IsNullOrEmpty(doctorSearch.CRM))
            {
                query = query.Where(x => x.CRM.Contains(doctorSearch.CRM));
            }

            return query;
        }

        #endregion
    }
}
