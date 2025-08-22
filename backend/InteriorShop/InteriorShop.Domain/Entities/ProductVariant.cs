using InteriorShop.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InteriorShop.Domain.Entities
{
    public class ProductVariant : BaseEntity
    {
        public Guid ProductId { get; set; }
        public Product Product { get; set; } = default!;
        public string Sku { get; set; } = default!;
        public decimal Price { get; set; }
        public decimal? SalePrice { get; set; }
        public int Stock { get; set; }

        public bool IsActive { get; set; } = true;   // bật/tắt variant
        public int SortOrder { get; set; } = 0;      // sắp xếp thứ tự

        // VD: {"Chân bàn":"Đen","Kích thước":"140x70","Mặt bàn":"Vân gỗ"}
        public string OptionsJson { get; set; } = "{}";
    }
}
