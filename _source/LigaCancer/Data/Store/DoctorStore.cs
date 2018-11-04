using LigaCancer.Code;
using LigaCancer.Code.Interface;
using LigaCancer.Data.Models.PatientModels;
using LigaCancer.Code.Requests;
using LigaCancer.Code.Responses;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Store
{
    public class DoctorStore : IDataStore<Doctor>, IDataTable<Doctor>
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
                Doctor doctor = _context.Doctors.Include(x => x.PatientInformationDoctors).FirstOrDefault(b => b.DoctorId == model.DoctorId);
                if (doctor.PatientInformationDoctors.Count > 0)
                {
                    result.Errors.Add(new TaskError
                    {
                        Code = "Acesso Negado",
                        Description = "Não é possível apagar este médico"
                    });
                    return Task.FromResult(result);
                }
                doctor.IsDeleted = true;
                doctor.DeletedDate = DateTime.Now;
                doctor.CRM = DateTime.Now + "||" + doctor.CRM;
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

        public Task<Doctor> FindByIdAsync(string id, ISpecification<Doctor> specification = null)
        {
            if(specification != null)
            {
                return Task.FromResult(
                _context.Doctors
                .IncludeExpressions(specification.Includes)
                .IncludeByNames(specification.IncludeStrings)
                .FirstOrDefault(x => x.DoctorId == int.Parse(id)));
            }
            else
            {
                return Task.FromResult(_context.Doctors.FirstOrDefault(x => x.DoctorId == int.Parse(id)));
            }
            
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

        //IDataTable
        public async Task<DataTableResponse> GetOptionResponseWithSpec(DataTableOptions options, ISpecification<Doctor> spec)
        {
            var data = await _context.Set<Doctor>()
                            .IncludeExpressions(spec.Includes)
                            .IncludeByNames(spec.IncludeStrings)
                            .GetOptionResponseAsync(options);

            return data;
        }

        public async Task<DataTableResponse> GetOptionResponse(DataTableOptions options)
        {
            return await _context.Set<Doctor>().GetOptionResponseAsync(options);
        }

        #region Custom Methods

        public Task<Doctor> FindByCRMAsync(string crm, int DoctorId)
        {
            Doctor doctor = _context.Doctors.IgnoreQueryFilters().FirstOrDefault(x => x.CRM == crm && x.DoctorId != DoctorId);
            return Task.FromResult(doctor);
        }

        public Task<Doctor> FindByNameAsync(string Name)
        {
            Doctor doctor = _context.Doctors.FirstOrDefault(x => x.Name == Name);
            if (doctor != null && doctor.IsDeleted)
            {
                doctor.IsDeleted = false;
                doctor.LastUpdatedDate = DateTime.Now;
            }
            return Task.FromResult(doctor);
        }

        #endregion
    }
}
