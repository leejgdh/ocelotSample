using Ecremmoce.Enums;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ItemSDK.Models
{
    public class Item
    {

        [Key]
        public Guid ItemId { get; set; }

        /// <summary>
        /// 상품코드
        /// </summary>
        [Required]
        public string VariationSku { get; set; }

        /// <summary>
        /// 상품명
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// HSCode
        /// </summary>
        public string HsCode { get; set; }

        /// <summary>
        /// 안전재고
        /// </summary>
        public int SafetyStock { get; set; }

        /// <summary>
        /// 로케이션
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 바코드
        /// </summary>
        public string Barcode { get; set; }

        /// <summary>
        /// 무게
        /// </summary>
        public double? Weight { get; set; }

        /// <summary>
        /// 가로(cm)
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// 세로(cm)
        /// </summary>
        public double? Length { get; set; }

        /// <summary>
        /// 높이(cm)
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// 메모사항
        /// </summary>
        public string Memo { get; set; }

        /// <summary>
        /// 상품 판매 URL
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 썸네일 URL
        /// </summary>
        public string ThumbnailUrl { get; set; }

        /// <summary>
        /// 출처 (수집플랫폼 혹은 자체등록시엔 Null)
        /// </summary>

        [EnumDataType(typeof(EPlatform))]
        [JsonConverter(typeof(StringEnumConverter))]
        public EPlatform? Source { get; set; }

        [Required]
        public Guid CompanyId { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
