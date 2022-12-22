using BrandShop.Business.DTOs.WishDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BrandShopMVC.Controllers
{
    public class WishController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public WishController(AppDbContext context , UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> WishList()
        {
            WishListDto wishListVM = new WishListDto
            {
                WishListItems = await _getWishItems(),
            };
            return View(wishListVM);
        }

        private async Task<List<WishListItemDto>> _getWishItems()
        {
            List<WishListItemDto> wishlistItems = new List<WishListItemDto>();

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (user != null && user.IsAdmin == false)
            {
                List<WishItem> wishItems = _context.WishItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Include(d => d.Product).ThenInclude(d => d.Brand).Where(x => x.AppUserId == user.Id).ToList();

                foreach (var item in wishItems)
                {
                    WishListItemDto wishlistItem = new WishListItemDto
                    {
                        Product = item.Product,

                    };
                    wishlistItems.Add(wishlistItem);
                }
            }
            else
            {
                string wishItemsStr = HttpContext.Request.Cookies["wishItemList"];
                if (wishItemsStr != null)
                {
                    List<CookieWishItemDto> cookieWishItems = JsonConvert.DeserializeObject<List<CookieWishItemDto>>(wishItemsStr);

                    foreach (var item in cookieWishItems)
                    {
                        WishListItemDto checkoutItem = new WishListItemDto
                        {
                            Product = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).FirstOrDefault(x => x.Id == item.ProductId),

                        };
                        wishlistItems.Add(checkoutItem);
                    }
                }
            }

            return wishlistItems;
        }
    }
}
