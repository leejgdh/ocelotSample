using Ecremmoce.Models.Base;
using ItemSDK.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemSDK.Interfaces
{
    public interface IItemService
    {
        #region 기본 Interface

        /// <summary>
		/// 단건조회
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        Item Get(Guid id);

        /// <summary>
		/// 모든건 조회
		/// </summary>
		/// <returns></returns>
        IQueryable<Item> GetAll();

        /// <summary>
        /// 단건 입력
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<TaskBase<Item>> InsertAsync(Item item);

        /// <summary>
        /// 복수 건 입력
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<TaskBase<List<Item>>> InsertsAsync(List<Item> items);

        /// <summary>
        /// 단건 업데이트
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        Task<TaskBase<Item>> UpdateAsync(Item item);

        /// <summary>
        /// 복수 건 업데이트
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        Task<TaskBase<List<Item>>> UpdatesAsync(List<Item> items);

        /// <summary>
		/// 단건 삭제
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
        Task<TaskBase<Item>> DeleteAsync(Guid id);

        /// <summary>
		/// 복수 건 삭제
		/// </summary>
		/// <param name="iDs"></param>
		/// <returns></returns>
		Task<TaskBase<List<Item>>> DeletesAsync(List<Guid> iDs);

        #endregion


        /// <summary>
        /// CompanyId 기준으로 VariationSku일치하는 하나 상품 검색
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="VariationSku"></param>
        /// <returns></returns>
        Item Get(Guid companyId, string VariationSku);
    }
}
