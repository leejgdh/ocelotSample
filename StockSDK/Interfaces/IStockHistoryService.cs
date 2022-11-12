using Ecremmoce.Models.Base;
using StockSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSDK.Interfaces
{
    public interface IStockHistoryService
    {
        #region 기본 Interface

        StockHistory Get(Guid id);

        IQueryable<StockHistory> GetAll();

        Task<TaskBase<StockHistory>> InsertAsync(StockHistory stockHistory);

        Task<TaskBase<StockHistory>> UpdateAsync(StockHistory stockHistory);

        Task<TaskBase<StockHistory>> DeleteAsync(Guid id);


        Task<TaskBase<List<StockHistory>>> InsertsAsync(List<StockHistory> stockHistories);

        Task<TaskBase<List<StockHistory>>> UpdatesAsync(List<StockHistory> stockHistories);

        Task<TaskBase<List<StockHistory>>> DeletesAsync(List<Guid> Ids);

        #endregion
    }
}
