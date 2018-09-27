using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LigaCancer.Code
{
    public class PaginatedList<T> : List<T>
    {
        public int PageIndex { get; private set; }
        public int TotalPages { get; private set; }
        public int CountTotal { get; set; }
        public int InitValue { get; set; }
        public int EndValue { get; set; }

        public PaginatedList(List<T> items, int count, int countTotal, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            CountTotal = countTotal;
            InitValue = (pageIndex - 1) * pageSize + 1;
            EndValue = pageSize * pageIndex;
            if(EndValue > CountTotal)
            {
                EndValue = CountTotal;
            }
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            AddRange(items);
        }

        public bool HasPreviousPage
        {
            get
            {
                return (PageIndex > 1);
            }
        }

        public bool HasNextPage
        {
            get
            {
                return (PageIndex < TotalPages);
            }
        }

        public static async Task<PaginatedList<T>> CreateAsync(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int countTotal = source.Count();
            var count = await source.CountAsync();
            var items = await source.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return new PaginatedList<T>(items, count, countTotal, pageIndex, pageSize);
        }
    }
}
