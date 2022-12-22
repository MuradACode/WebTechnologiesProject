using BrandShop.Core.Entities;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.AuthenticateDto
{
    public class SignUpDto
    {
        public Guid Id { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }                                                                                                                                      
        public string RepeatPassword { get; set; }

    }

    public class SignUpDtoValidator : AbstractValidator<SignUpDto>
    {
        public SignUpDtoValidator()
        {
            RuleFor(x => x.FullName).NotNull().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.UserName).NotNull().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.PhoneNumber).NotNull().MaximumLength(10).MinimumLength(10);
            RuleFor(x => x.Address).NotNull().MinimumLength(20).MaximumLength(100);
            RuleFor(x => x.Email).NotNull().MaximumLength(100).MinimumLength(8);
            RuleFor(x => x.Password).NotNull().MinimumLength(8).MaximumLength(25);
            RuleFor(x => x.RepeatPassword).NotNull().Equal(x => x.Password);

        }
    }
}
