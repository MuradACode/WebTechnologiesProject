using BrandShop.Business.DTOs.CommentDto;
using BrandShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.ProductDto
{
    public class DetailDto
    {
        public Product Product { get; set; }
        public ProductComment Comment { get; set; }
        public CreateCommentDto CommentDto { get; set; }
        public List<Product> RelatedProduct { get; set; }
    }
}
