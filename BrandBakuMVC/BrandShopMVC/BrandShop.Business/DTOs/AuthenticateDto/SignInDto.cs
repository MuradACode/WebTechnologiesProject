using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.AuthenticateDto
{
    public class SignInDto
    {
        public string Email { get; set; }   
        public string Password { get; set; }
    }

    public class SignInDtoValidator : AbstractValidator<SignInDto>
    {
        public SignInDtoValidator()
        {
            RuleFor(x => x.Email).NotNull().MaximumLength(100).MinimumLength(8);
            RuleFor(x => x.Password).NotNull().MinimumLength(8).MaximumLength(25);

        }
    }
}
