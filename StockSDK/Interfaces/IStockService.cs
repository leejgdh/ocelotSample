using Ecremmoce.Models.Base;
using StockSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StockSDK.Interfaces
{
    public interface IStockService
    {
        #region 기본 Interface

        /// <summary>
        /// 단건조회
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Stock Get(Guid id);

        /// <summary>
		/// 모든건 조회
		/// </summary>
		/// <returns></returns>
        IQueryable<Stock> GetAll();

        /// <summary>
        /// 단건 입력
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        Task<TaskBase<Stock>> InsertAsync(Stock stock);

        /// <summary>
        /// 복수 건 입력
        /// </summary>
        /// <param name="stocks"></param>
        /// <returns></returns>
        Task<TaskBase<List<Stock>>> InsertsAsync(List<Stock> stocks);

        /// <summary>
        /// 단건 업데이트
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        Task<TaskBase<Stock>> UpdateAsync(Stock stock);

        /// <summary>
        /// 복수 건 업데이트
        /// </summary>
        /// <param name="stocks"></param>
        /// <returns></returns>
        Task<TaskBase<List<Stock>>> UpdatesAsync(List<Stock> stocks);

        /// <summary>
		/// 단건 삭제
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        Task<TaskBase<Stock>> DeleteAsync(Guid id);

        /// <summary>
		/// 복수 건 삭제
		/// </summary>
		/// <param name="iDs"></param>
		/// <returns></returns>
        Task<TaskBase<List<Stock>>> DeletesAsync(List<Guid> Ids);

        #endregion

        /// <summary>
        /// Id 없이 CompanyId, VariationSku로 단건 조회
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="VariationSku"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Stock Get(Guid companyId, string VariationSku);
    }
}
