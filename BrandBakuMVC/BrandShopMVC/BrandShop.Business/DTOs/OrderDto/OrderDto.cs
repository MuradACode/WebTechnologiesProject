using BrandShop.Business.DTOs.ProductDto;
using BrandShop.Core.Entities;
using BrandShop.Core.Enums;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BrandShop.Business.DTOs.OrderDto
{
    public class OrderDto
    {
        //AppUser
        public BasketDto BasketDto { get; set; }
        public AppUser Users { get; set; }
        public string AppUserId { get; set; }
        public Order Order { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public OrderStatus Status { get; set; }
        public OrderDeliveryStatus? DeliveryStatus { get; set; }
        public decimal TotalAmount { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public string RejectComment { get; set; }
        public List<OrderItemDto> OrderItems { get; set; }
    }

    public class QuestionDtoValidator : AbstractValidator<OrderDto>
    {
        public QuestionDtoValidator()
        {
            RuleFor(x => x.FullName).NotNull().MaximumLength(50).MinimumLength(3);
            RuleFor(x => x.Email).NotNull().MaximumLength(100).MinimumLength(8);
            RuleFor(x => x.Address).NotNull().MinimumLength(20).MaximumLength(100);
            RuleFor(x => x.Phone).NotNull().MaximumLength(10).MinimumLength(10);
        }
    }
}
