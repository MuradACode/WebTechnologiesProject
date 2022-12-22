using BrandShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.HomeDto
{
    public class HomeIndexDto
    {
        public Hero Heroes { get; set; }
        public List<Brand> Brands { get; set; }
        public List<Service> Services { get; set; }
        public Discounted Discounteds { get; set; }
        public List<Product> IsTrendProd { get; set; }
        public List<Product> IsBestProd { get; set; }
        public List<Product> IsSmartProd { get; set; }
        public List<Product> IsLiked { get;set; }
        public Product IsDiscProd { get; set; }
        public QuestionDto Question { get; set; }
    }
}
