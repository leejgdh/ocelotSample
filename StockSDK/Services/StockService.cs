using Ecremmoce.Helper;
using Ecremmoce.Models.Base;
using Microsoft.Extensions.Logging;
using StockSDK.Interfaces;
using StockSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSDK.Services
{
    public class StockService : IStockService
    {
        #region #. private members

        private readonly ILogger<StockService> _logger;
        private readonly StockContext _context;

        #endregion

        #region #. constructor

        public StockService(
            ILogger<StockService> logger,
            StockContext context)
        {
            _logger = logger;
            _context = context;
        }

        #endregion

        #region #. properties

        //  한글주석 encoding 깨짐 현상 테스트....

        #endregion

        #region #. methods

        private TaskBase<Stock> Delete(Guid id)
        {
            TaskBase<Stock> result = new TaskBase<Stock>();

            try
            {
                var data = Get(id);
                if (data == null)
                {
                    result.IsSuccess = true;
                    result.Message = $"Cannot find {nameof(Stock)}";
                    return result;
                }

                _context.Stocks.Remove(data);

                result.IsSuccess = true;
                result.Result = data;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Exception = e;
                result.Message = e.Message;
            }

            return result;
        }

        public async Task<TaskBase<Stock>> DeleteAsync(Guid id)
        {
            TaskBase<Stock> result = new TaskBase<Stock>();

            try
            {
                result = Delete(id);


                if (result.IsSuccess)
                {
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;

                if (e.InnerException != null)
                {
                    result.Message = e.InnerException.Message;
                }
                else
                {
                    result.Message = e.Message;
                }

            }

            return result;
        }

        public Stock Get(Guid id)
        {
            var data = _context.Stocks.FirstOrDefault(e => e.Id == id);

            return data;
        }

        public Stock Get(Guid companyId, string VariationSku)
        {
            var data = _context.Stocks.Where(e => e.CompanyId == companyId)
                .FirstOrDefault(e => e.VariationSku == VariationSku);

            return data;
        }

        public IQueryable<Stock> GetAll()
        {
            var data = _context.Stocks.AsQueryable();

            return data;
        }

        private TaskBase<Stock> Insert(Stock stock)
        {
            TaskBase<Stock> result = new TaskBase<Stock>();

            try
            {
                stock.CreatedAt = DateTime.UtcNow;

                _context.Stocks.Add(stock);

                result.IsSuccess = true;
                result.Result = stock;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Exception = e;
                result.Message = e.Message;
            }

            return result;
        }

        public async Task<TaskBase<Stock>> InsertAsync(Stock stock)
        {
            TaskBase<Stock> result = new TaskBase<Stock>();

            try
            {
                result = Insert(stock);


                if (result.IsSuccess)
                {
                    await _context.SaveChangesAsync();
                    result.Result = stock;
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;

                if (e.InnerException != null)
                {
                    result.Message = e.InnerException.Message;
                }
                else
                {
                    result.Message = e.Message;
                }

            }

            return result;
        }

        private TaskBase<Stock> Update(Stock stock)
        {
            TaskBase<Stock> result = new TaskBase<Stock>();

            try
            {
                var entity = Get(stock.Id);
                if (entity == null)
                {
                    result.IsSuccess = false;
                    result.Message = $"Cannot find {nameof(Stock)}";
                    return result;
                }

                PropertyHelper.UpdatePropertyFromNameExclude(ref entity, stock, new string[]
                {
                    nameof(Stock.StockHistories)
                });

                stock.UpdatedAt = DateTime.UtcNow;

                result.IsSuccess = true;
                result.Result = entity;

                return result;

            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Exception = e;
                result.Message = e.Message;
            }

            return result;
        }

        public async Task<TaskBase<Stock>> UpdateAsync(Stock stock)
        {
            TaskBase<Stock> result = new TaskBase<Stock>();

            try
            {
                result = Update(stock);


                if (result.IsSuccess)
                {
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;

                if (e.InnerException != null)
                {
                    result.Message = e.InnerException.Message;
                }
                else
                {
                    result.Message = e.Message;
                }

            }

            return result;
        }

        public async Task<TaskBase<List<Stock>>> DeletesAsync(List<Guid> ids)
        {
            TaskBase<List<Stock>> result = new TaskBase<List<Stock>>();

            try
            {
                var deleteReses = ids.Select(e => Delete(e));
                var successes = deleteReses.Where(e => e.IsSuccess);
                var fails = deleteReses.Where(e => !e.IsSuccess);

                if (fails.Any())
                {
                    result.IsSuccess = false;
                    result.Result = fails.Select(e => e.Result).ToList();
                    result.Message = string.Join(",", fails.Select(e => e.Message));
                }
                else
                {
                    await _context.SaveChangesAsync();
                    result.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;

                if (e.InnerException != null)
                {
                    result.Message = e.InnerException.Message;
                }
                else
                {
                    result.Message = e.Message;
                }

            }

            return result;
        }

        public async Task<TaskBase<List<Stock>>> InsertsAsync(List<Stock> stocks)
        {
            TaskBase<List<Stock>> result = new TaskBase<List<Stock>>();

            try
            {
                var insertReses = stocks.Select(e => Insert(e));
                var successes = insertReses.Where(e => e.IsSuccess);
                var fails = insertReses.Where(e => !e.IsSuccess);

                if (fails.Any())
                {
                    result.IsSuccess = false;
                    result.Result = fails.Select(e => e.Result).ToList();
                    result.Message = string.Join(",", fails.Select(e => e.Message));
                }
                else
                {
                    await _context.SaveChangesAsync();
                    result.Result = stocks;
                    result.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;

                if (e.InnerException != null)
                {
                    result.Message = e.InnerException.Message;
                }
                else
                {
                    result.Message = e.Message;
                }

            }

            return result;
        }

        public async Task<TaskBase<List<Stock>>> UpdatesAsync(List<Stock> stocks)
        {
            TaskBase<List<Stock>> result = new TaskBase<List<Stock>>();

            try
            {
                var updateReses = stocks.Select(e => Update(e));
                var successes = updateReses.Where(e => e.IsSuccess);
                var fails = updateReses.Where(e => !e.IsSuccess);

                if (fails.Any())
                {
                    result.IsSuccess = false;
                    result.Result = fails.Select(e => e.Result).ToList();
                    result.Message = string.Join(",", fails.Select(e => e.Message));
                }
                else
                {
                    await _context.SaveChangesAsync();
                    result.IsSuccess = true;
                }
            }
            catch (Exception e)
            {
                result.IsSuccess = false;

                if (e.InnerException != null)
                {
                    result.Message = e.InnerException.Message;
                }
                else
                {
                    result.Message = e.Message;
                }

            }

            return result;
        }

        #endregion
    }
}
