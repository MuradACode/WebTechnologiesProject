using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.AuthenticateDto
{
    public class ForgotDto
    {
        public string Email { get; set; }

        public class ForgotDtoValidator : AbstractValidator<ForgotDto>
        {
            public ForgotDtoValidator()
            {
                RuleFor(x => x.Email).NotNull().MaximumLength(100).MinimumLength(8);

            }
        }
    }
}
