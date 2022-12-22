using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Core.Entities
{
    public class Hero:BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string BtnText { get; set; }
        public string BtnLink { get; set; }
        public string Image { get; set; }
        public string BackgroundImage { get; set; }
        public IFormFile BgImageFile { get; set; }
        public IFormFile ImageFile { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
