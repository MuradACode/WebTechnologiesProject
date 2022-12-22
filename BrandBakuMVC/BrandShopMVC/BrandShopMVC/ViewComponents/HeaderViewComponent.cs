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
    public class HeaderViewComponent:ViewComponent
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HeaderViewComponent(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            BasketDto basket = null;

            AppUser user = null;

            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }


            if (user != null && user.IsAdmin == false)
            {
                basket = _getBasketItems(_context.BasketItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Where(x => x.AppUserId == user.Id).ToList());
            }
            else
            {
                var basketItemsStr = HttpContext.Request.Cookies["basketItemList"];

                if (basketItemsStr != null)
                {
                    List<CookieBasketItemDto> cookieItems = JsonConvert.DeserializeObject<List<CookieBasketItemDto>>(basketItemsStr);
                    basket = _getBasketItems(cookieItems);
                }
            }


            HeaderDto headerVM = new HeaderDto
            {
                Basket = basket
            };
            return View(headerVM);
        }

        private BasketDto _getBasketItems(List<CookieBasketItemDto> cookieBasketItems)
        {
            BasketDto basket = new BasketDto
            {
                BasketItems = new List<BasketItemDto>(),
            };

            foreach (var item in cookieBasketItems)
            {
                Product product = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Include(x => x.ProductColors).ThenInclude(x => x.Color).FirstOrDefault(x => x.Id == item.ProductId);
                BasketItemDto basketItem = new BasketItemDto
                {
                    Brand = product.Brand.Name,
                    Model = product.Model,
                    Price = product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice,
                    ProductId = product.Id,
                    Count = item.Count,
                    PosterImage = product.ProductImages.FirstOrDefault(x => x.PosterStatus == true)?.Image
                };

                basketItem.TotalPrice = basketItem.Count * basketItem.Price;
                basket.TotalAmount += basketItem.TotalPrice;
                basket.BasketItems.Add(basketItem);
            }

            return basket;
        }

        private BasketDto _getBasketItems(List<BasketItem> basketItems)
        {
            BasketDto basket = new BasketDto
            {
                BasketItems = new List<BasketItemDto>(),
            };

            foreach (var item in basketItems)
            {
                BasketItemDto basketItem = new BasketItemDto
                {
                    Brand = item.Product.Brand.Name,
                    Model = item.Product.Model,
                    Price = item.Product.DiscountPercent > 0 ? (item.Product.SalePrice * (1 - item.Product.DiscountPercent / 100)) : item.Product.SalePrice,
                    ProductId = item.Product.Id,
                    Count = item.Count,
                    PosterImage = item.Product.ProductImages.FirstOrDefault(x => x.PosterStatus == true)?.Image
                };

                basketItem.TotalPrice = basketItem.Count * basketItem.Price;
                basket.TotalAmount += basketItem.TotalPrice;
                basket.BasketItems.Add(basketItem);
            }

            return basket;
        }
    }
}
