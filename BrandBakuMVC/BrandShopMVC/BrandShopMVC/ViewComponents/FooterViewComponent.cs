using BrandShop.Business.DTOs.FooterDto;
using BrandShop.Business.DTOs.HeaderDto;
using BrandShop.Business.DTOs.ProductDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BrandShopMVC.ViewComponents
{
    public class FooterViewComponent : ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public FooterViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            FooterDto footerDto = new FooterDto
            {
                Brand = _context.Brands.ToList(),
            };
            return View(footerDto);
        }
    }
}
