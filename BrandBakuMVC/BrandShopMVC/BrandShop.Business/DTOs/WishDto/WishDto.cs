using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.WishDto
{
    public class WishDto
    {
        public List<WishItemDto> WishItems { get; set; }
    }

    public class WishItemDto
    {
        public int ProductId { get; set; }
        public string Model { get; set; }
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }
        public decimal DiscountPercent { get; set; }
        public string Brand { get; set; }
    }
}
