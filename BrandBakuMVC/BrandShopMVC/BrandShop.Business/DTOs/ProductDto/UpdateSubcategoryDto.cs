using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.ProductDto
{
    public class UpdateSubcategoryDto
    {
        public string Name { get; set; }
        public int CategoryId { get; set; }
    }

    public class UpdateSubcategoryDtoValidator : AbstractValidator<CreateSubcategoryDto>
    {
        public UpdateSubcategoryDtoValidator()
        {
            RuleFor(x => x.Name).NotNull().MinimumLength(3).MaximumLength(30);
            RuleFor(x => x.CategoryId).NotNull();
        }
    }
}
