﻿using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.AuthenticateDto
{
    public class ResetDto
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public string Password { get; set; }
        public string RepeatPassword { get; set; }

        public class ResetDtoValidator : AbstractValidator<ResetDto>
        {
            public ResetDtoValidator()
            {
                RuleFor(x => x.Email).NotNull().MaximumLength(100).MinimumLength(8);
                RuleFor(x => x.Token).NotNull();
                RuleFor(x => x.Password).NotNull().MinimumLength(8).MaximumLength(25);
                RuleFor(x => x.RepeatPassword).NotNull().Equal(x => x.Password);

            }
        }
    }
}
