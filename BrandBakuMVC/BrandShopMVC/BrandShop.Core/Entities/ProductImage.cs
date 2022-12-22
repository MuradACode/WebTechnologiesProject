using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Core.Entities
{
    public class ProductImage:BaseEntity
    {
        public string Image { get; set; }
        public bool? PosterStatus { get; set; }

        //Product
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
