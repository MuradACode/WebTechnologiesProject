using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.ProductDto
{
    public class UpdateColorDto
    {
        public string Name { get; set; }
        public string ColorHexCode { get; set; }
    }

    public class UpdateColorDtoValidator:AbstractValidator<UpdateColorDto>
    {
        public UpdateColorDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().MinimumLength(3).MaximumLength(20);
            RuleFor(x => x.ColorHexCode).NotNull().MinimumLength(7).MaximumLength(7);
        }
    }
}
