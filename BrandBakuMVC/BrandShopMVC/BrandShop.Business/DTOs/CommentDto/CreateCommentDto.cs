using BrandShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.CommentDto
{
    public class CreateCommentDto
    {
        public string Text { get; set; }
        public string Rate { get; set; }
        //public string FullName { get; set; }
        //public string Email { get; set; }
        public bool Status { get; set; }

        //Product 
        public int ProductId { get; set; }
        //public Product Product { get; set; }
    }
}
