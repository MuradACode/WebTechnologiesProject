using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.ProductDto
{
    public class CreateColorDto
    {
        public string Name { get; set; }
        public string ColorHexCode { get; set; }
    }

    public class CreateColorDtoValidator:AbstractValidator<CreateColorDto>
    {
        public CreateColorDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().MinimumLength(3).MaximumLength(20);
            RuleFor(x => x.ColorHexCode).NotNull().MinimumLength(7).MaximumLength(7);
        }
    }
}
