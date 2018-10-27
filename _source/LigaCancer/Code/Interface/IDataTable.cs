using LigaCancer.Data.Requests;
using LigaCancer.Data.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Code.Interface
{
    interface IDataTable<T> : IDisposable where T : class
    {
        Task<DataTableResponse> GetOptionResponse(DataTableOptions options);

        Task<DataTableResponse> GetOptionResponseWithSpec(DataTableOptions options, ISpecification<T> spec);
    }
}
