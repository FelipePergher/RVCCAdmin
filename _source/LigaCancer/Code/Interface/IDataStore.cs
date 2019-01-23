using LigaCancer.Code;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Code.Interface
{
    public interface IDataStore<T> : IDisposable where T : class
    {
        Task<List<T>> GetAllAsync(string[] include = null, int take = int.MaxValue, int skip = 0);

        Task<T> FindByIdAsync(string id, ISpecification<T> specification = null);

        Task<TaskResult> CreateAsync(T model);

        Task<TaskResult> UpdateAsync(T model);

        Task<TaskResult> DeleteAsync(T model);

        int Count();

    }
}
