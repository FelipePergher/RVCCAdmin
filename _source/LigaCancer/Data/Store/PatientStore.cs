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
    public class PatientStore : IDataStore<Patient>, IDataTable<Patient>
    {
        private readonly ApplicationDbContext _context;

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
                if (patient != null)
                {
                    patient.IsDeleted = true;
                    patient.DeletedDate = DateTime.Now;
                    _context.Update(patient);
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

        public Task<Patient> FindByIdAsync(string id, ISpecification<Patient> specification = null, bool ignoreQueryFilter = false)
        {
            IQueryable<Patient> queryable = _context.Patients;
            if (ignoreQueryFilter)
            {
                queryable = queryable.IgnoreQueryFilters();
            }

            if (specification != null)
            {
                queryable = queryable.IncludeExpressions(specification.Includes).IncludeByNames(specification.IncludeStrings);
            }

            return Task.FromResult(queryable.FirstOrDefault(x => x.PatientId == int.Parse(id)));
        }

        public Task<List<Patient>> GetAllAsync(string[] include = null)
        {
            IQueryable<Patient> query = _context.Patients;

            if (include != null)
            {
                query = include.Aggregate(query, (current, inc) => current.Include(inc));
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

        //DataTable Methods
        public async Task<DataTableResponse> GetOptionResponseWithSpec(DataTableOptions options, ISpecification<Patient> specification)
        {
            DataTableResponse data = await _context.Set<Patient>()
                            .IncludeExpressions(specification.Includes)
                            .IncludeWheres(specification.Wheres)
                            .IncludeByNames(specification.IncludeStrings)
                            .GetOptionResponseAsync(options);

            return data;
        }

        public async Task<DataTableResponse> GetOptionResponseWithSpecIgnoreQueryFilter(DataTableOptions options, ISpecification<Patient> specification)
        {
            DataTableResponse data = await _context.Set<Patient>()
                            .IncludeExpressions(specification.Includes)
                            .IncludeWheres(specification.Wheres)
                            .IncludeByNames(specification.IncludeStrings)
                            .IgnoreQueryFilters()
                            .GetOptionResponseAsync(options);

            return data;
        }

        public async Task<DataTableResponse> GetOptionResponse(DataTableOptions options)
        {
            return await _context.Set<Patient>().GetOptionResponseAsync(options);
        }

        #region Custom Methods

        public Task<Patient> FindByCpfAsync(string cpf, int patientId)
        {
            Patient patient = _context.Patients.IgnoreQueryFilters().FirstOrDefault(x => x.CPF == cpf && x.PatientId != patientId);
            return Task.FromResult(patient);
        }

        public Task<Patient> FindByRgAsync(string rg, int patientId)
        {
            Patient patient = _context.Patients.IgnoreQueryFilters().FirstOrDefault(x => x.RG == rg && x.PatientId != patientId);
            return Task.FromResult(patient);
        }

        public TaskResult ActivePatient(Patient patient)
        {
            TaskResult result = new TaskResult();
            try
            {
                patient.IsDeleted = false;
                patient.DeletedDate = DateTime.MinValue;
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

            return result;
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
                BaseSpecification<Patient> specification = new BaseSpecification<Patient>(x => x.Family, x => x.Family.FamilyMembers);
                Patient patient = await FindByIdAsync(patientId, specification);
                if (patient.Family == null)
                {
                    patient.Family = new Family();
                }
                patient.Family.FamilyIncome += familyMember.MonthlyIncome;
                patient.Family.FamilyMembers.Add(familyMember);

                patient.Family.PerCapitaIncome = patient.Family.FamilyIncome / (patient.Family.FamilyMembers.Count() + 1);

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
