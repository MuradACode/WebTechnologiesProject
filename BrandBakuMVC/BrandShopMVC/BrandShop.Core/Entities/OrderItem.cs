using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Core.Entities
{
    public class OrderItem:BaseEntity
    {
        //Order
        public int OrderId { get; set; }
        public Order Order { get; set; }

        //Product
        public int ProductId { get; set; }
        public Product Product { get; set; }
        
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int DiscountPercent { get; set; }
        public int Count { get; set; }
    }
}
