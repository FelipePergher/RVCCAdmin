using LigaCancer.Code.Requests;
using LigaCancer.Code.Responses;
using System;
using System.Threading.Tasks;

namespace LigaCancer.Code.Interface
{
    public interface IDataTable<T> : IDisposable where T : class
    {
        Task<DataTableResponse> GetOptionResponse(DataTableOptions options);

        Task<DataTableResponse> GetOptionResponseWithSpec(DataTableOptions options, ISpecification<T> specification);
    }
}
