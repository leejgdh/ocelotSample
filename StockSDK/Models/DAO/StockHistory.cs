using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace StockSDK.Models
{
    public class StockHistory
    {

        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        /// <summary>
        /// 입고
        /// </summary>
        public int In { get; set; }

        /// <summary>
        /// 출고
        /// </summary>
        public int Out { get; set; }

        /// <summary>
        /// 사유
        /// </summary>
        [StringLength(200)]
        public string Reason { get; set; }

        /// <summary>
        /// 메모
        /// </summary>
        [StringLength(1000)]
        public string Memo { get; set; }

        /// <summary>
        /// 변동일자
        /// </summary>
        public DateTime UpdateDate { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }

        [Required]
        public Guid StockId { get; set; }

        public Stock Stock { get; set; }
    }
}
