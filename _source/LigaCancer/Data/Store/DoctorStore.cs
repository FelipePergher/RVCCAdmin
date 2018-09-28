using LigaCancer.Code;
using LigaCancer.Data.Models.PatientModels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Store
{
    public class DoctorStore : IDataStore<Doctor>
    {
        private ApplicationDbContext _context;

        public DoctorStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Doctors.Count();
        }

        public Task<TaskResult> CreateAsync(Doctor model)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Doctors.Add(model);
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

        public Task<TaskResult> DeleteAsync(Doctor model)
        {
            TaskResult result = new TaskResult();
            try
            {
                Doctor doctor = _context.Doctors.FirstOrDefault(b => b.DoctorId == model.DoctorId);
                doctor.IsDeleted = true;
                doctor.DeletedDate = DateTime.Now;
                doctor.CRM = "deleted-" + doctor.CRM;
                _context.Update(doctor);

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

        public Task<Doctor> FindByIdAsync(string id, string[] include = null)
        {
            IQueryable<Doctor> query = _context.Doctors;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.FirstOrDefault(x => x.DoctorId == int.Parse(id)));
        }

        public Task<List<Doctor>> GetAllAsync(string[] include = null)
        {
            IQueryable<Doctor> query = _context.Doctors;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

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

        public IQueryable<Doctor> GetAllQueryable(string[] include = null)
        {
            IQueryable<Doctor> query = _context.Doctors;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return query;
        }

        #region

        public Task<Doctor> FindByCRMAsync(string crm, int DoctorId)
        {
            Doctor doctor = _context.Doctors.IgnoreQueryFilters().FirstOrDefault(x => x.CRM == crm && x.DoctorId != DoctorId);
            return Task.FromResult(doctor);
        }

        #endregion
    }
}
