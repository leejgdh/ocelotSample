using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockSDK.Models
{
    public class Stock
    {
        [Key]
        public Guid Id { get; set; }

        /// <summary>
        /// 상품코드
        /// </summary>
        [Required]
        [StringLength(200)]
        public string VariationSku { get; set; }

        /// <summary>
        /// 현재재고
        /// </summary>
        public int NowStock { get; set; }

        [Required]
        public Guid ItemId { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        public List<StockHistory> StockHistories { get; set; }
    }
}
