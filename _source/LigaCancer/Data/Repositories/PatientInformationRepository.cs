// <copyright file="PatientInformationRepository.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Data.Repositories
{
    public class PatientInformationRepository : IDataRepository<PatientInformation>
    {
        private readonly ApplicationDbContext _context;

        public PatientInformationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.PatientInformation.Count();
        }

        public Task<TaskResult> CreateAsync(PatientInformation patientInformation)
        {
            var result = new TaskResult();
            try
            {
                _context.PatientInformation.Add(patientInformation);
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

        public Task<TaskResult> DeleteAsync(PatientInformation patientInformation)
        {
            var result = new TaskResult();
            try
            {
                _context.PatientInformation.Remove(patientInformation);
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

        public Task<PatientInformation> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<PatientInformation> query = _context.PatientInformation;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.PatientInformationId == int.Parse(id)));
        }

        public Task<List<PatientInformation>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            throw new NotImplementedException();
        }

        public Task<TaskResult> UpdateAsync(PatientInformation patientInformation)
        {
            var result = new TaskResult();
            try
            {
                patientInformation.UpdatedTime = DateTime.Now;
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
