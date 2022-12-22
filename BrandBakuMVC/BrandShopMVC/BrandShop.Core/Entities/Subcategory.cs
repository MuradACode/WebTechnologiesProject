using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Core.Entities
{
    public class Subcategory:BaseEntity
    {
        public string Name { get; set; }
        
        //Category
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; } 
        public DateTime? DeletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Category Category { get; set; }
    }
}
