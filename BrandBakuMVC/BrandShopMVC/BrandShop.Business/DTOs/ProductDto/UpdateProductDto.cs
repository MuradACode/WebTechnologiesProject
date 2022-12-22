using BrandShop.Core.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.ProductDto
{
    public class UpdateProductDto
    {
        public string Model { get; set; }
        public string Description { get; set; }
        public decimal CostPrice { get; set; }
        public decimal SalePrice { get; set; }
        public int DiscountPercent { get; set; }
        public bool StockStatus { get; set; }
        public bool IsTrend { get; set; }
        public bool IsBest { get; set; }
        public bool IsSmart { get; set; }
        public bool IsRelated { get; set; }
        public bool IsLiked { get; set; }
        public bool IsDiscounted { get; set; }
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime? UpdatedAt { get; set; }
        //Category
        public int CategoryId { get; set; }
        public int SubcategoryId { get; set; }

        //Color
        //public List<ProductColor> ProductColors { get; set; }
        public List<int> ColorIds { get; set; }

        //Brand
        public int BrandId { get; set; }

        //Product Image
        public List<ProductImage> ProductImages { get; set; }
        public IFormFile? PosterFile { get; set; }
        public List<IFormFile>? ImageFiles { get; set; }
    }

    public class UpdateProductDtoValidator : AbstractValidator<UpdateProductDto>
    {
        public UpdateProductDtoValidator()
        {
            RuleFor(x => x.Model).NotNull().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.Description).NotNull().MinimumLength(20).MaximumLength(250);
            RuleFor(x => x.CostPrice).NotNull().LessThanOrEqualTo(100000).GreaterThanOrEqualTo(20);
            RuleFor(x => x.SalePrice).NotNull().LessThanOrEqualTo(100000).GreaterThanOrEqualTo(20);
            RuleFor(x => x.DiscountPercent).NotNull().LessThanOrEqualTo(100).GreaterThanOrEqualTo(0);
            RuleFor(x => x.StockStatus).NotNull();
            RuleFor(x => x.IsLiked).NotNull();
            RuleFor(x => x.IsBest).NotNull();
            RuleFor(x => x.IsSmart).NotNull();
            RuleFor(x => x.IsTrend).NotNull();
            RuleFor(x => x.IsDiscounted).NotNull();
            RuleFor(x => x.IsRelated).NotNull();
        }
    }
}
