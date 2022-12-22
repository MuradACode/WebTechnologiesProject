using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.ProductDto
{
    public class BasketDto
    {
        public decimal TotalAmount { get; set; }
        public List<BasketItemDto> BasketItems { get; set; }
    }
    public class BasketItemDto
    {
        public int Count { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public string Brand { get; set; }
        public string Model { get; set; }
        public string PosterImage { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
