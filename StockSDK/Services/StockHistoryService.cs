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
    public class StockHistoryService : IStockHistoryService
    {
        #region #. private members

        private readonly ILogger<StockHistoryService> _logger;
        private readonly StockContext _context;

        #endregion

        #region #. constructor

        public StockHistoryService(
            ILogger<StockHistoryService> logger,
            StockContext context)
        {
            _logger = logger;
            _context = context;
        }

        #endregion

        #region #. properties


        #endregion

        #region #. methods

        public StockHistory Get(Guid id)
        {
            var entity = _context.StockHistories.FirstOrDefault(e => e.Id == id);
            return entity;
        }

        public IQueryable<StockHistory> GetAll()
        {
            var entities = _context.StockHistories.AsQueryable();
            return entities;
        }

        private TaskBase<StockHistory> Insert(StockHistory stockHistory)
        {
            TaskBase<StockHistory> result = new TaskBase<StockHistory>();

            DateTime utcnow = DateTime.UtcNow;

            try
            {
                var stock = _context.Stocks
                    .FirstOrDefault(e => e.Id == stockHistory.StockId);

                if (stock == null)
                {
                    throw new Exception("DB Error: mismatched stockId");
                }

                stockHistory.CreatedAt = utcnow;

                _context.StockHistories.Add(stockHistory);

                stock.NowStock += stockHistory.In;
                stock.NowStock -= stockHistory.Out;
                stock.UpdatedAt = utcnow;

                result.IsSuccess = true;
                result.Result = stockHistory;
            }
            catch (Exception e)
            {
                result.IsSuccess = false;
                result.Exception = e;
                result.Message = e.Message;
            }

            return result;


        }

        public async Task<TaskBase<StockHistory>> InsertAsync(StockHistory stockHistory)
        {
            TaskBase<StockHistory> result = new TaskBase<StockHistory>();

            try
            {
                result = Insert(stockHistory);


                if (result.IsSuccess)
                {
                    await _context.SaveChangesAsync();
                    result.Result = stockHistory;
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

        public async Task<TaskBase<List<StockHistory>>> InsertsAsync(List<StockHistory> stockHistories)
        {
            TaskBase<List<StockHistory>> result = new TaskBase<List<StockHistory>>();

            try
            {
                var insertReses = stockHistories.Select(e => Insert(e));
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
                    result.Result = stockHistories;
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

        private TaskBase<StockHistory> Update(StockHistory stockHistory)
        {
            TaskBase<StockHistory> result = new TaskBase<StockHistory>();

            try
            {
                var entity = Get(stockHistory.Id);
                if (entity == null)
                {
                    result.IsSuccess = false;
                    result.Message = $"Cannot find {nameof(StockHistory)}";
                    return result;
                }

                PropertyHelper.UpdatePropertyFromNameExclude(ref entity, stockHistory, new string[]
                {
                    nameof(StockHistory.Stock)
                });

                stockHistory.UpdatedAt = DateTime.UtcNow;

                result.IsSuccess = true;
                result.Result = entity;

                return result;

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Update Error");

                result.IsSuccess = false;
                result.Exception = e;
                result.Message = e.Message;
            }

            return result;

        }

        public async Task<TaskBase<StockHistory>> UpdateAsync(StockHistory stockHistory)
        {
            TaskBase<StockHistory> result = new TaskBase<StockHistory>();

            try
            {
                result = Update(stockHistory);

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

        public async Task<TaskBase<List<StockHistory>>> UpdatesAsync(List<StockHistory> stockHistories)
        {
            TaskBase<List<StockHistory>> result = new TaskBase<List<StockHistory>>();

            try
            {
                var updateReses = stockHistories.Select(e => Update(e));
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

        private TaskBase<StockHistory> Delete(Guid id)
        {
            TaskBase<StockHistory> result = new TaskBase<StockHistory>();

            try
            {
                var data = Get(id);
                if (data == null)
                {
                    result.IsSuccess = true;
                    result.Message = $"Cannot find {nameof(StockHistory)}";
                    return result;
                }

                _context.StockHistories.Remove(data);

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

        public async Task<TaskBase<StockHistory>> DeleteAsync(Guid id)
        {
            TaskBase<StockHistory> result = new TaskBase<StockHistory>();

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

        public async Task<TaskBase<List<StockHistory>>> DeletesAsync(List<Guid> ids)
        {
            TaskBase<List<StockHistory>> result = new TaskBase<List<StockHistory>>();

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

        #endregion

    }
}
