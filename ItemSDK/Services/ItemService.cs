using Ecremmoce.Helper;
using Ecremmoce.Models.Base;
using ItemSDK.Interfaces;
using ItemSDK.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItemSDK.Services
{
	public class ItemService : IItemService
	{
		#region #. private members

		private readonly ILogger<ItemService> _logger;
		private readonly ItemContext _context;

		#endregion

		#region #. constructor

		public ItemService(
			ILogger<ItemService> logger,
			ItemContext context
			)
		{
			_logger = logger;
			_context = context;
		}

		#endregion

		#region #. properties

		#endregion

		#region #. methods

		public Item Get(Guid id)
		{

			var data = _context.Items
				.FirstOrDefault(e => e.ItemId == id);

			return data;
		}

		public Item Get(Guid companyId, string VariationSku)
		{
			var data = _context.Items.FirstOrDefault(e => e.CompanyId == companyId && e.VariationSku == VariationSku);

			return data;
		}

		public IQueryable<Item> GetAll()
		{
			var datas = _context.Items.AsQueryable();

			return datas;
		}

		private TaskBase<Item> Insert(Item item)
		{
			TaskBase<Item> result = new TaskBase<Item>();

			try
			{
				item.CreatedAt = DateTime.Now.ToUniversalTime();

				_context.Items.Add(item);


				result.IsSuccess = true;
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
				result.Result = item;
			}


			return result;
		}


		public async Task<TaskBase<Item>> InsertAsync(Item item)
		{
			TaskBase<Item> result = new TaskBase<Item>();

			try
			{
				result = Insert(item);

				if (result.IsSuccess)
				{
					await _context.SaveChangesAsync();
					result.Result = item;
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

		public async Task<TaskBase<List<Item>>> InsertsAsync(List<Item> items)
		{
			TaskBase<List<Item>> result = new TaskBase<List<Item>>();

			try
			{
				var inserts = items.Select(e => Insert(e));

				var successes = inserts.Where(e => e.IsSuccess);
				var fails = inserts.Where(e => !e.IsSuccess);

				if (fails.Any())
				{
					result.IsSuccess = false;
					result.Result = fails.Select(e => e.Result).ToList();
					result.Message = string.Join(",", fails.Select(e => e.Message));
				}
				else
				{
					await _context.SaveChangesAsync();
					result.Result = items;
					result.IsSuccess = true;
				}

				return result;
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

		public async Task<TaskBase<List<Item>>> SyncsAsync(List<Item> items)
		{
			TaskBase<List<Item>> result = new TaskBase<List<Item>>();

			try
			{
				foreach (var item in items)
				{
					var entity = Get(item.CompanyId, item.VariationSku);

					if (entity != null)
					{
						await UpdateAsync(item);
					}
					else
					{
						await InsertAsync(item);
					}
				}

				result.IsSuccess = true;
				result.Result = items;

			}
			catch (Exception e)
			{
				result.IsSuccess = false;
				result.Message = e.Message;
			}

			return result;
		}



		private TaskBase<Item> Update(Item item)
		{
			TaskBase<Item> result = new TaskBase<Item>();

			var entity = item.ItemId != Guid.Empty ? Get(item.ItemId) : Get(item.CompanyId, item.VariationSku);

			try
			{
				if (entity != null)
				{
					PropertyHelper.UpdatePropertyFromNameExclude(ref entity, item, new string[] {
							nameof(item.ItemId),
							nameof(item.VariationSku),
							nameof(item.CompanyId),
							nameof(item.CreatedAt)

						});

					// 여기도 업데이트 할 때 UpdatedAt 현재시간으로 넣는걸로 (by 이준규 프로)
					item.UpdatedAt = DateTime.Now.ToUniversalTime();

					result.IsSuccess = true;
					result.Result = entity;
				}
				else
				{
					result.IsSuccess = false;
					result.Message = $"Cannot find {nameof(Item)}";
				}

			}
			catch (Exception e)
			{
				result.IsSuccess = false;
				result.Message = e.Message;
			}



			return result;
		}



		public async Task<TaskBase<Item>> UpdateAsync(Item item)
		{
			TaskBase<Item> result = new TaskBase<Item>();

			try
			{
				result = Update(item);

				if (result.IsSuccess)
				{
					await _context.SaveChangesAsync();
				}

			}
			catch (Exception e)
			{
				result.IsSuccess = false;
				result.Message = e.Message;
			}


			return result;
		}

		public async Task<TaskBase<List<Item>>> UpdatesAsync(List<Item> items)
		{
			TaskBase<List<Item>> result = new TaskBase<List<Item>>();

			try
			{
				var updates = items.Select(e => Update(e));

				var successes = updates.Where(e => e.IsSuccess);
				var fails = updates.Where(e => !e.IsSuccess);

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


				return result;

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

		private TaskBase<Item> Delete(Guid id)
		{
			TaskBase<Item> result = new TaskBase<Item>();

			try
			{
				var data = Get(id);

				if (data != null)
				{

					_context.Items.Remove(data);

					result.IsSuccess = true;
					result.Result = data;
				}
				else
				{
					result.IsSuccess = true;
					result.Message = $"Cannot find {nameof(Item)}";
				}

			}
			catch (Exception e)
			{
				result.IsSuccess = false;
				result.Message = e.Message;
			}

			return result;
		}



		public async Task<TaskBase<Item>> DeleteAsync(Guid id)
		{
			TaskBase<Item> result = new TaskBase<Item>();

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
				result.Message = e.Message;
			}

			return result;
		}

		public async Task<TaskBase<List<Item>>> DeletesAsync(List<Guid> ids)
		{
			TaskBase<List<Item>> result = new TaskBase<List<Item>>();

			try
			{

				var deletes = ids.Select(e => Delete(e));

				var successes = deletes.Where(e => e.IsSuccess);
				var fails = deletes.Where(e => !e.IsSuccess);


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

				return result;
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
