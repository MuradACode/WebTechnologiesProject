using BrandShop.Business.DTOs.OrderDto;
using BrandShop.Business.DTOs.ProductDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BrandShopMVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public OrderController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> CheckOut()
        {
            OrderDto orderDto = new OrderDto
            {
                OrderItems = await _getCheckoutItems(),
                Order = new Order()
            };
            return View(orderDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(OrderDto order)
        {


            AppUser user = _userManager.Users.FirstOrDefault(x => x.IsAdmin == false && x.UserName == User.Identity.Name);
            if (user == null)
            {
                return RedirectToAction("signin", "authenticate");
            }


            List<OrderItemDto> checkoutItems = await _getCheckoutItems();

            if (checkoutItems.Count == 0)
            {
                ModelState.AddModelError("", "There is not any selected product");
            }

            if (!ModelState.IsValid)
            {
                return View("Checkout", new OrderDto { OrderItems = checkoutItems });
            }

            var lastOrder = _context.Orders.OrderByDescending(x => x.Id).FirstOrDefault();
            //order.CreatedAt = DateTime.UtcNow.AddHours(4);
            order.Status = BrandShop.Core.Enums.OrderStatus.Pending;
            order.DeliveryStatus = BrandShop.Core.Enums.OrderDeliveryStatus.OnProcessing;
            order.OrderItems = new List<OrderItemDto>();
            //order.AppUserId = user.Id;
            foreach (var item in checkoutItems)
            {
                OrderItemDto orderItem = new OrderItemDto
                {
                    
                    ProductId = item.Product.Id,
                    CostPrice = item.Product.CostPrice,
                    Count = item.Count,
                    SalePrice = item.Product.SalePrice,
                    DiscountPercent = item.Product.DiscountPercent
                };
                order.TotalAmount += orderItem.DiscountPercent > 0
                ? orderItem.SalePrice * (1 - orderItem.DiscountPercent / 100) * orderItem.Count
                : orderItem.SalePrice * orderItem.Count;
                order.OrderItems.Add(orderItem);
            }

            _context.Orders.Add(lastOrder);
            _context.BasketItems.RemoveRange(_context.BasketItems.Where(x => x.AppUserId == user.Id));
            _context.SaveChanges();
            return RedirectToAction("profile", "account");
        }

        private async Task<List<OrderItemDto>> _getCheckoutItems()
        {
            List<OrderItemDto> checkoutItems = new List<OrderItemDto>();

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (user != null && user.IsAdmin == false)
            {
                List<BasketItem> basketItems = _context.BasketItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Include(x => x.Product).ThenInclude(x => x.Brand).Where(x => x.AppUserId == user.Id).ToList();

                foreach (var item in basketItems)
                {
                    OrderItemDto checkoutItem = new OrderItemDto
                    {
                        Product = item.Product,
                        Count = item.Count
                    };
                    checkoutItems.Add(checkoutItem);
                }
            }
            else
            {
                string basketItemsStr = HttpContext.Request.Cookies["basketItemList"];
                if (basketItemsStr != null)
                {
                    List<CookieBasketItemDto> basketItems = JsonConvert.DeserializeObject<List<CookieBasketItemDto>>(basketItemsStr);

                    foreach (var item in basketItems)
                    {
                        OrderItemDto checkoutItem = new OrderItemDto
                        {
                            Product = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).FirstOrDefault(x => x.Id == item.ProductId),
                            Count = item.Count
                        };
                        checkoutItems.Add(checkoutItem);
                    }
                }
            }

            return checkoutItems;
        }
    }
}
