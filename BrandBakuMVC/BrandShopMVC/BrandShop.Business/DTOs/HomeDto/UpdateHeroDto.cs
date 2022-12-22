using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.HomeDto
{
    public class UpdateHeroDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string BtnText { get; set; }
        public string BtnLink { get; set; } 
        public string? Image { get; set; }
        public string? BackgroundImage { get; set; }
        public IFormFile? BgImageFile { get; set; }
        public IFormFile? ImageFile { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }

    public class UpdateHeroDtoValidator : AbstractValidator<UpdateHeroDto>
    {
        public UpdateHeroDtoValidator()
        {
            RuleFor(x => x.Title).NotNull().MinimumLength(3).MaximumLength(30);
            RuleFor(x => x.Description).NotNull().MinimumLength(20).MaximumLength(200);
            RuleFor(x => x.BtnText).NotNull().MinimumLength(3).MaximumLength(15);
            RuleFor(x => x.BtnLink).NotNull().MinimumLength(20).MaximumLength(500);
        }   
    }
}

