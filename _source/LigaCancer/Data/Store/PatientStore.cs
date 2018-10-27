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
    public class PatientStore : IDataStore<Patient>
    {
        private ApplicationDbContext _context;

        public PatientStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Patients.Count();
        }

        public Task<TaskResult> CreateAsync(Patient model)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Patients.Add(model);
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

        public Task<TaskResult> DeleteAsync(Patient model)
        {
            TaskResult result = new TaskResult();
            try
            {
                Patient patient = _context.Patients.FirstOrDefault(b => b.PatientId == model.PatientId);
                patient.IsDeleted = true;
                patient.DeletedDate = DateTime.Now;
                _context.Update(patient);

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

        public Task<Patient> FindByIdAsync(string id, string[] include = null)
        {
            IQueryable<Patient> query = _context.Patients;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.FirstOrDefault(x => x.PatientId == int.Parse(id)));
        }

        public Task<List<Patient>> GetAllAsync(string[] include = null)
        {
            IQueryable<Patient> query = _context.Patients;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Patient model)
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

        public IQueryable<Patient> GetAllQueryable(string[] include = null)
        {
            IQueryable<Patient> query = _context.Patients;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return query;
        }

        #region Custom Methods

        public Task<Patient> FindByCpfAsync(string Cpf, int PatientId)
        {
            Patient patient = _context.Patients.IgnoreQueryFilters().FirstOrDefault(x => x.CPF == Cpf && x.PatientId != PatientId);
            return Task.FromResult(patient);
        }

        public Task<Patient> FindByRgAsync(string Rg, int PatientId)
        {
            Patient patient = _context.Patients.IgnoreQueryFilters().FirstOrDefault(x => x.RG == Rg && x.PatientId != PatientId);
            return Task.FromResult(patient);
        }

        public async Task<TaskResult> AddPhone(Phone phone, string patientId)
        {
            TaskResult result = new TaskResult();

            try
            {
                Patient patient = await FindByIdAsync(patientId);
                patient.Phones.Add(phone);
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

            return result;
        }

        public async Task<TaskResult> AddAddress(Address address, string patientId)
        {
            TaskResult result = new TaskResult();

            try
            {
                Patient patient = await FindByIdAsync(patientId);
                patient.Addresses.Add(address);
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

            return result;
        }

        public async Task<TaskResult> AddFamilyMember(FamilyMember familyMember, string patientId)
        {
            TaskResult result = new TaskResult();

            try
            {
                Patient patient = await FindByIdAsync(patientId, new string[] { "Family", "Family.FamilyMembers" });
                if(patient.Family == null)
                {
                    patient.Family = new Family();
                }
                patient.Family.FamilyIncome += familyMember.MonthlyIncome;
                patient.Family.FamilyMembers.Add(familyMember);

                patient.Family.PerCapitaIncome = patient.Family.FamilyIncome / patient.Family.FamilyMembers.Count();

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

            return result;
        }

        public async Task<TaskResult> AddFileAttachment(FileAttachment fileAttachment, string patientId)
        {
            TaskResult result = new TaskResult();

            try
            {
                Patient patient = await FindByIdAsync(patientId);
                patient.FileAttachments.Add(fileAttachment);

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

            return result;
        }

        #endregion
    }
}
