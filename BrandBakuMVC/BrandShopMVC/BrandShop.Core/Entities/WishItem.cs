using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Core.Entities
{
    public class WishItem:BaseEntity
    {
        //AppUser
        public string AppUserId { get; set; }
        public AppUser AppUser { get;set; }
        
        //Product
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
