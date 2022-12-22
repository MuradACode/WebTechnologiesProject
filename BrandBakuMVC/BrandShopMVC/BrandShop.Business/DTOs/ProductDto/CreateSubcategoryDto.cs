using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.ProductDto
{
    public class CreateSubcategoryDto
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }

    public class CreateSubcategoryDtoValidator : AbstractValidator<CreateSubcategoryDto>
    {
        public CreateSubcategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().MinimumLength(3).MaximumLength(30);
            RuleFor(x => x.CategoryId).NotNull();
        }
    }
}
