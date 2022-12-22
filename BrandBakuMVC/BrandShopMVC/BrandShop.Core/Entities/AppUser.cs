using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Core.Entities
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }

        public bool IsAdmin { get; set; }

        public string Address { get;set; }

        //Audit info
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        //Deletable info
        public bool IsDeleted { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
