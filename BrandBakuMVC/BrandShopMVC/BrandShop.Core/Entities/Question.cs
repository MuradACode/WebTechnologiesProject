using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Core.Entities
{
    public class Question:BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }
        
    }
}
