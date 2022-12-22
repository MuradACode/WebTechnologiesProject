using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.ProductDto
{
    public class UpdateBrandDto
    {
        public string Name { get; set; }
        public string Link { get; set; }
        public string? Image { get; set; }
        public IFormFile? ImageFile { get; set; }
    }

    public class UpdateBrandDtoValidator:AbstractValidator<UpdateBrandDto>
    {
        public UpdateBrandDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().MinimumLength(3).MaximumLength(30);
            RuleFor(x => x.Link).NotNull().MinimumLength(10).MaximumLength(100);
        }
    }
}
