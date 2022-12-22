using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.ServiceDto
{
    public class UpdateServiceDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Icon { get; set; }
    }

    public class UpdateServiceDtoValidator:AbstractValidator<UpdateServiceDto>
    {
        public UpdateServiceDtoValidator()
        {
            RuleFor(x => x.Title).NotNull().MinimumLength(10).MaximumLength(50);
            RuleFor(x => x.Description).NotNull().MinimumLength(10).MaximumLength(100);
            RuleFor(x => x.Icon).NotNull().MinimumLength(8).MaximumLength(100);
        }
    }
}
