using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Data.Store
{
    public interface IDataStore<T> : IDisposable where T : class
    {
        Task<List<T>> GetAllAsync(string[] include = null);

        Task<T> FindByIdAsync(string id, string[] include = null);

        Task<T> CreateAsync(T model);

        Task<T> UpdateAsync(T model);

        Task<T> DeleteAsync(T model);

        int Count();

    }
}
