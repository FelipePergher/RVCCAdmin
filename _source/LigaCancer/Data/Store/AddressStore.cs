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
    public class AddressStore : IDataStore<Address>
    {
        private ApplicationDbContext _context;

        public AddressStore(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.Addresses.Count();
        }

        public Task<TaskResult> CreateAsync(Address model)
        {
            TaskResult result = new TaskResult();
            try
            {
                _context.Addresses.Add(model);
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

        public Task<TaskResult> DeleteAsync(Address model)
        {
            TaskResult result = new TaskResult();
            try
            {
                Address address = _context.Addresses.FirstOrDefault(b => b.AddressId == model.AddressId);
                address.IsDeleted = true;
                address.DeletedDate = DateTime.Now;
                _context.Update(address);

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

        public Task<Address> FindByIdAsync(string id, string[] include = null)
        {
            IQueryable<Address> query = _context.Addresses;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.FirstOrDefault(x => x.AddressId == int.Parse(id)));
        }

        public Task<List<Address>> GetAllAsync(string[] include = null)
        {
            IQueryable<Address> query = _context.Addresses;

            if (include != null)
            {
                foreach (var inc in include)
                {
                    query = query.Include(inc);
                }
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(Address model)
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

    }
}
