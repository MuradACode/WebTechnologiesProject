using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.ProductDto
{
    public class UpdateCategoryDto
    {
        public string Name { get; set; }
    }

    public class UpdateCategoryValidatorDto:AbstractValidator<UpdateCategoryDto>
    {
        public UpdateCategoryValidatorDto()
        {
            RuleFor(x => x.Name).NotNull().MinimumLength(3).MaximumLength(30);
        }
    }
}
