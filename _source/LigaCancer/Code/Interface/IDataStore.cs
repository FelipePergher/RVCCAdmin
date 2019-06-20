﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LigaCancer.Code.Interface
{
    public interface IDataStore<T> : IDisposable where T : class
    {
        Task<List<T>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null);

        Task<T> FindByIdAsync(string id, string[] includes = null);

        Task<TaskResult> CreateAsync(T model);

        Task<TaskResult> UpdateAsync(T model);

        Task<TaskResult> DeleteAsync(T model);

        int Count();

    }
}
