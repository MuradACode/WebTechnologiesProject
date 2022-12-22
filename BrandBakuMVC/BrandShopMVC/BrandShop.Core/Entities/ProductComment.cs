using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Core.Entities
{
    public class ProductComment:BaseEntity
    {
        public string Text { get; set; }
        public string Rate { get; set; }
        public bool Status { get; set; }
        public bool CommentStatus { get; set; }
        
        //AppUser
        public string AppUserId { get; set; }
        public AppUser User { get; set; }

        //Product 
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
