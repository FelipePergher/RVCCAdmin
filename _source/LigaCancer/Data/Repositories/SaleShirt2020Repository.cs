// <copyright file="SaleShirt2020Repository.cs" company="Doffs">
// Copyright (c) Doffs. All Rights Reserved.
// </copyright>

using Microsoft.EntityFrameworkCore;
using RVCC.Business;
using RVCC.Business.Interface;
using RVCC.Data.Models;
using RVCC.Models.SearchModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RVCC.Data.Repositories
{
    public class SaleShirt2020Repository : IDataRepository<SaleShirt2020>
    {
        private readonly ApplicationDbContext _context;

        public SaleShirt2020Repository(ApplicationDbContext context)
        {
            _context = context;
        }

        public int Count()
        {
            return _context.SalesShirt2020.Count();
        }

        public Task<TaskResult> CreateAsync(SaleShirt2020 saleShirt2020)
        {
            var result = new TaskResult();
            try
            {
                _context.SalesShirt2020.Add(saleShirt2020);
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

        public Task<TaskResult> DeleteAsync(SaleShirt2020 saleShirt2020)
        {
            var result = new TaskResult();
            try
            {
                _context.SalesShirt2020.Remove(saleShirt2020);
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

        public Task<SaleShirt2020> FindByIdAsync(string id, string[] includes = null)
        {
            IQueryable<SaleShirt2020> query = _context.SalesShirt2020;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            return Task.FromResult(query.FirstOrDefault(x => x.ShirtSaleId == int.Parse(id)));
        }

        public Task<List<SaleShirt2020>> GetAllAsync(string[] includes = null, string sortColumn = "", string sortDirection = "", object filter = null)
        {
            IQueryable<SaleShirt2020> query = _context.SalesShirt2020;

            if (includes != null)
            {
                query = includes.Aggregate(query, (current, inc) => current.Include(inc));
            }

            if (!string.IsNullOrEmpty(sortColumn) && !string.IsNullOrEmpty(sortDirection))
            {
                query = GetOrdinationSaleShirt2020(query, sortColumn, sortDirection);
            }

            if (filter != null)
            {
                query = GetFilteredSaleShirt2020s(query, (SaleShirt2020SearchModel)filter);
            }

            return Task.FromResult(query.ToList());
        }

        public Task<TaskResult> UpdateAsync(SaleShirt2020 saleShirt2020)
        {
            var result = new TaskResult();
            try
            {
                saleShirt2020.UpdatedTime = DateTime.Now;
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

        #region Private Methods

        private IQueryable<SaleShirt2020> GetOrdinationSaleShirt2020(IQueryable<SaleShirt2020> query, string sortColumn, string sortDirection)
        {
            return sortColumn switch
            {
                "Status" => sortDirection == "asc"
                    ? query.OrderByDescending(x => x.Status)
                    : query.OrderBy(x => x.Status),
                "BuyerName" => sortDirection == "asc"
                    ? query.OrderBy(x => x.BuyerName)
                    : query.OrderByDescending(x => x.BuyerName),
                "Date" => sortDirection == "asc"
                    ? query.OrderBy(x => x.DateOrdered)
                    : query.OrderByDescending(x => x.DateOrdered),
                "MaskQuantity" => sortDirection == "asc"
                    ? query.OrderBy(x => x.MaskQuantity)
                    : query.OrderByDescending(x => x.MaskQuantity),
                "ShirtQuantityTotal" => sortDirection == "asc"
                    ? query.OrderBy(x => x.ShirtQuantityTotal)
                    : query.OrderByDescending(x => x.ShirtQuantityTotal),
                "PriceTotal" => sortDirection == "asc"
                    ? query.OrderBy(x => x.PriceTotal)
                    : query.OrderByDescending(x => x.PriceTotal),
                _ => sortDirection == "asc" ? query.OrderBy(x => "rvcc" + x.ShirtSaleId) : query.OrderByDescending(x => "rvcc" + x.ShirtSaleId)
            };
        }

        private IQueryable<SaleShirt2020> GetFilteredSaleShirt2020s(IQueryable<SaleShirt2020> query, SaleShirt2020SearchModel saleShirt2020Search)
        {
            if (!string.IsNullOrEmpty(saleShirt2020Search.Code))
            {
                query = query.Where(x => ("rvcc" + x.ShirtSaleId).Contains(saleShirt2020Search.Code.ToLower()));
            }

            if (!string.IsNullOrEmpty(saleShirt2020Search.Name))
            {
                query = query.Where(x => x.BuyerName.ToLower().Contains(saleShirt2020Search.Name.ToLower()));
            }

            if (saleShirt2020Search.States != null && saleShirt2020Search.States.Any())
            {
                query = query.Where(x => saleShirt2020Search.States.Contains(x.Status));
            }

            return query;
        }

        #endregion
    }
}
