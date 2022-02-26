// <copyright file="IDataRepository.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using System.Collections.Generic;
using System.Threading.Tasks;

namespace RVCC.Business.Interface
{
    public interface IDataRepository<T>
    {
        Task<List<T>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null);

        Task<T> FindByIdAsync(string id, string[] includes = null);

        Task<TaskResult> CreateAsync(T model);

        Task<TaskResult> UpdateAsync(T model);

        Task<TaskResult> DeleteAsync(T model);

        int Count();
    }
}
