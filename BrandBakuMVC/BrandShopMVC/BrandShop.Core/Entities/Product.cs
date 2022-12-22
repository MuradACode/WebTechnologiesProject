using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Core.Entities
{
    public class Product : BaseEntity
    {
        public string Model { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int DiscountPercent { get; set; }
        public bool StockStatus { get; set; }
        public bool IsTrend { get; set; }
        public bool IsBest { get; set; }
        public bool IsSmart { get; set; }
        public bool IsRelated { get; set; }
        public bool IsLiked { get; set; }
        public bool IsDiscounted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? UpdatedAt { get; set; }
        //Category
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        //Subcategory
        public int SubcategoryId { get; set; }
        public List<Subcategory> Subcategory { get; set; }

        //Color
        public List<ProductColor> ProductColors { get; set; }
        public List<int> ColorIds { get; set; }

        //Brand
        public int BrandId { get; set; }
        public Brand Brand { get; set; }

        //Product Image
        public List<ProductImage> ProductImages { get; set; }
        public IFormFile PosterFile { get; set; }
        public List<IFormFile> ImageFiles { get; set; }

        //Comments
        public List<ProductComment> ProductComments { get; set; }



    }
}
