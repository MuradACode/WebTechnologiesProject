using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.HomeDto
{
    public class QuestionDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Text { get; set; }

        public class QuestionDtoValidator : AbstractValidator<QuestionDto>
        {
            public QuestionDtoValidator()
            {
                RuleFor(x => x.FullName).NotNull().MaximumLength(50).MinimumLength(3);
                RuleFor(x => x.Email).NotNull().MaximumLength(100).MinimumLength(8);
                RuleFor(x => x.Text).NotNull().MaximumLength(500).MinimumLength(30);
            }
        }
    }
}
