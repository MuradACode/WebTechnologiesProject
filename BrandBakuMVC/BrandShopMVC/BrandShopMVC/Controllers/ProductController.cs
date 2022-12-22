using BrandShop.Business.DTOs.CommentDto;
using BrandShop.Business.DTOs.ProductDto;
using BrandShop.Business.DTOs.WishDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BrandShopMVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public ProductController(AppDbContext context, UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Shop(int? categoryId, int? colorId, decimal? minPrice = null, decimal? maxPrice = null)
        {

            var products = _context.Products
                .Include(x => x.Category)
                .Include(x => x.Brand)
                .Include(x => x.ProductImages)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .Where(x => x.IsDeleted == false)
                .AsQueryable();

            if (categoryId != null)
            {
                products = products.Where(x => x.CategoryId == categoryId);
            }

            ViewBag.MinPrice = (int)products.Min(x => x.SalePrice);
            ViewBag.MaxPrice = (int)products.Max(x => x.SalePrice);

            if (colorId != null)
            {
                products = products.Where(x => x.ProductColors.Any(d => d.ColorId == colorId));
            }

            if (minPrice != null && maxPrice != null)
            {
                products = products.Where(x => x.SalePrice >= minPrice && x.SalePrice <= maxPrice);
            }


            ViewBag.SelectedMinPrice = minPrice ?? ViewBag.MinPrice;
            ViewBag.SelectedMaxPrice = maxPrice ?? ViewBag.MaxPrice;

            ShopDto shopDto = new ShopDto
            {
                CategoryId = categoryId,
                ColorId = colorId,
                Categories = _context.Categories.Where(x => x.IsDeleted == false).ToList(),
                Subcategories = _context.Subcategories.Where(x => x.IsDeleted == false).ToList(),
                Brands = _context.Brands.Where(x => x.IsDeleted == false).ToList(),
                Colors = _context.Colors.ToList(),
                Products = products
                .Include(x => x.ProductImages)
                .Include(x => x.Brand)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color).ToList(),
            };
            return View(shopDto);
        }

        public async Task<IActionResult> Detail(int id)
        {
            ViewBag.ProductId = id;
            Product product = await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.Brand)
                .Include(x => x.ProductComments)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .Where(x => x.IsDeleted == false)
                .FirstOrDefaultAsync(x => x.Id == id);

            DetailDto detailDto = new DetailDto
            {
                Product = product,
                CommentDto = new CreateCommentDto(),
                RelatedProduct = _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.Brand)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .Where(x => x.IsDeleted == false)
                .ToList(),
            };

            detailDto.CommentDto.ProductId = product.Id;

            return View(detailDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Comment(CreateCommentDto commentDto)
        {
            //return Ok(commentDto);

            var product = await _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.Brand)
                .Include(x => x.ProductComments)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .Where(x => x.IsDeleted == false && x.Id == commentDto.ProductId)
                .FirstOrDefaultAsync();

            if (product == null) return NotFound();

            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            ProductComment comment = new ProductComment
            {
                Text = commentDto.Text,
                Rate = commentDto.Rate,
                Status = false,
                ProductId = commentDto.ProductId,
                AppUserId = user.Id
            };

            DetailDto detailDto = new DetailDto
            {
                Product = product,
                Comment = comment,
                RelatedProduct = _context.Products
                .Include(x => x.ProductImages)
                .Include(x => x.Brand)
                .Include(x => x.ProductColors)
                .ThenInclude(x => x.Color)
                .Where(x => x.IsDeleted == false)
                .ToList(),
            };

            if (!ModelState.IsValid)
            {
                TempData["error"] = "Comment model is not valid!";

                return View("Detail", detailDto);
            }

            if (!_context.Products.Any(x => x.Id == comment.ProductId))
            {
                TempData["error"] = "Selected Product not found";
                return View("Detail", detailDto);
            }

            _context.ProductComments.Add(comment);
            _context.SaveChanges();

            TempData["Success"] = "Comment added successfully";

            return RedirectToAction("detail", new { Id = comment.ProductId });
        }

        public async Task<IActionResult> AddWish(int id)
        {
            if (!_context.Products.Any(x => x.Id == id))
            {
                return RedirectToAction("error", "error");
            }
            WishDto wishData = null;
            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (user != null && user.IsAdmin == false)
            {

                WishItem wishItem = _context.WishItems.FirstOrDefault(x => x.AppUserId == user.Id && x.ProductId == id);

                if (wishItem == null)
                {
                    wishItem = new WishItem
                    {
                        AppUserId = user.Id,
                        ProductId = id,
                    };
                    _context.WishItems.Add(wishItem);
                }

                _context.SaveChanges();

                wishData = _getWishItems(_context.WishItems.Include(x => x.Product).ThenInclude(x => x.Brand).Where(x => x.AppUserId == user.Id).ToList());

            }
            else
            {
                List<CookieWishItemDto> wishItems = new List<CookieWishItemDto>();
                string existWishItem = HttpContext.Request.Cookies["wishItemList"];
                if (existWishItem != null)
                {
                    wishItems = JsonConvert.DeserializeObject<List<CookieWishItemDto>>(existWishItem);
                }
                CookieWishItemDto item = wishItems.FirstOrDefault(x => x.ProductId == id);
                if (item == null)
                {
                    item = new CookieWishItemDto
                    {
                        ProductId = id,
                    };
                    wishItems.Add(item);
                }

                var productIdStr = JsonConvert.SerializeObject(wishItems);
                HttpContext.Response.Cookies.Append("wishItemList", productIdStr);
                wishData = _getWishItems(wishItems);
            }
            TempData["Success"] = "Məhsul bəyəndiklərimə əlavə olundu.";

            return RedirectToAction("wishlist", "wish");
        }

        public async Task<IActionResult> DeleteWish(int id)
        {
            AppUser user = _userManager.Users.FirstOrDefault(x => x.UserName == User.Identity.Name && x.IsAdmin == false);
            if (!_context.Products.Any(x => x.Id == id))
            {
                return RedirectToAction("error", "error");
            }
            List<WishItemDto> wishItems = new List<WishItemDto>();
            if (user != null && !user.IsAdmin)
            {
                WishItem wishItem = _context.WishItems.FirstOrDefault(x => x.ProductId == id);
                if (wishItem == null)
                {
                    return RedirectToAction("error", "error");
                }

                _context.WishItems.Remove(wishItem);
                _context.SaveChanges();
            }
            else
            {
                string wish = HttpContext.Request.Cookies["wishItemList"];
                wishItems = JsonConvert.DeserializeObject<List<WishItemDto>>(wish);
                WishItemDto productWish = wishItems.FirstOrDefault(x => x.ProductId == id);
                if (productWish == null)
                {
                    return RedirectToAction("error", "error");
                }

                wishItems.Remove(productWish);

                HttpContext.Response.Cookies.Append("wishItemList", JsonConvert.SerializeObject(wishItems));
            }

            TempData["Error"] = "Məhsul bəyəndiklərimdən silindi.";
            return RedirectToAction("wishlist", "wish");

        }

        private WishDto _getWishItems(List<CookieWishItemDto> cookieWishItems)
        {

            WishDto wishItems = new WishDto()
            {
                WishItems = new List<WishItemDto>(),
            };
            foreach (var item in cookieWishItems)
            {
                Product product = _context.Products.Include(x => x.Brand).FirstOrDefault(x => x.Id == item.ProductId);
                WishItemDto wishItem = new WishItemDto
                {
                    Brand = product.Brand.Name,
                    Model = product.Model,
                    Price = (product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice),
                    SalePrice = product.SalePrice,
                    ProductId = product.Id,
                    DiscountPercent = product.DiscountPercent,
                };

            }
            return wishItems;

        }
        private WishDto _getWishItems(List<WishItem> wishItems)
        {
            WishDto wish = new WishDto
            {
                WishItems = new List<WishItemDto>(),
            };
            foreach (var item in wishItems)
            {
                WishItemDto wishItem = new WishItemDto
                {
                    Brand = item.Product.Brand.Name,
                    Model = item.Product.Model,
                    Price = item.Product.DiscountPercent > 0 ? (item.Product.SalePrice * (1 - item.Product.DiscountPercent / 100)) : item.Product.SalePrice,
                    ProductId = item.Product.Id,
                };
                wish.WishItems.Add(wishItem);
            }
            return wish;
        }

        public async Task<IActionResult> AddBasket(int prodId)
        {
            if (!_context.Products.Any(x => x.Id == prodId))
            {
                return NotFound();
            }

            BasketDto data = null;

            AppUser user = null;
            if (User.Identity.IsAuthenticated)
            {
                user = await _userManager.FindByNameAsync(User.Identity.Name);
            }

            if (user != null && user.IsAdmin == false)
            {
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.AppUserId == user.Id && x.ProductId == prodId);

                if (basketItem == null)
                {
                    basketItem = new BasketItem
                    {
                        AppUserId = user.Id,
                        ProductId = prodId,
                        Count = 1
                    };
                    _context.BasketItems.Add(basketItem);
                }
                else
                {
                    basketItem.Count++;
                }

                _context.SaveChanges();

                data = _getBasketItems(_context.BasketItems.Include(x => x.Product).ThenInclude(x => x.ProductImages).Include(x => x.Product).ThenInclude(x => x.Brand).Include(x => x.Product).ThenInclude(x => x.Category).Where(x => x.AppUserId == user.Id).ToList());
            }
            else
            {
                List<CookieBasketItemDto> basketItems = new List<CookieBasketItemDto>();
                string existBasketItems = HttpContext.Request.Cookies["basketItemList"];

                if (existBasketItems != null)
                {
                    basketItems = JsonConvert.DeserializeObject<List<CookieBasketItemDto>>(existBasketItems);
                }

                CookieBasketItemDto item = basketItems.FirstOrDefault(x => x.ProductId == prodId);

                if (item == null)
                {
                    item = new CookieBasketItemDto
                    {
                        ProductId = prodId,
                        Count = 1
                    };
                    basketItems.Add(item);
                }
                else
                    item.Count++;

                var prodIdsStr = JsonConvert.SerializeObject(basketItems);

                HttpContext.Response.Cookies.Append("basketItemList", prodIdsStr);

                data = _getBasketItems(basketItems);
            }

            TempData["Success"] = "Product add basket";
            //return Ok(data);
            return RedirectToAction("index", "home");
        }

        private BasketDto _getBasketItems(List<CookieBasketItemDto> cookieBasketItems)
        {
            BasketDto basket = new BasketDto
            {
                BasketItems = new List<BasketItemDto>(),
            };

            foreach (var item in cookieBasketItems)
            {
                Product product = _context.Products.Include(x => x.ProductImages).Include(x => x.Category).Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.ProductColors).ThenInclude(x => x.Color).FirstOrDefault(x => x.Id == item.ProductId);
                BasketItemDto basketItem = new BasketItemDto
                {
                    Brand = product.Brand.Name,
                    Model = product.Model,
                    Price = product.DiscountPercent > 0 ? (product.SalePrice * (1 - product.DiscountPercent / 100)) : product.SalePrice,
                    ProductId = product.Id,
                    Count = item.Count,
                    PosterImage = product.ProductImages.FirstOrDefault(x => x.PosterStatus == true)?.Image
                };

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

        public async Task<IActionResult> DeleteBasket(int id)
        {
            if (!_context.Products.Any(x => x.Id == id))
            {
                return RedirectToAction("index", "error");
            }
            List<BasketItemDto> productsDetail = new List<BasketItemDto>();

            if (User.Identity.IsAuthenticated)
            {
                BasketItem basketItem = _context.BasketItems.FirstOrDefault(x => x.ProductId == id);
                if (basketItem == null)
                {
                    return RedirectToAction("index", "error");
                }
                if (basketItem.Count == 1)
                {
                    _context.BasketItems.Remove(basketItem);
                }
                else
                {
                    basketItem.Count--;
                }
                _context.SaveChanges();
            }
            else
            {
                string basket = HttpContext.Request.Cookies["basketItemList"];
                productsDetail = JsonConvert.DeserializeObject<List<BasketItemDto>>(basket);
                BasketItemDto productBasket = productsDetail.FirstOrDefault(x => x.ProductId == id);
                if (productBasket == null)
                {
                    return RedirectToAction("index", "error");
                }
                if (productBasket.Count == 1)
                {
                    productsDetail.Remove(productBasket);
                }
                else
                {
                    productBasket.Count--;
                }
                HttpContext.Response.Cookies.Append("basketItemList", JsonConvert.SerializeObject(productsDetail));


            }
            return RedirectToAction("checkout", "order");

        }


        //[HttpPost]
        //public async Task<IActionResult> Comment(ProductComment comment)
        //{
        //    Product product = _context.Products
        //       .Include(x => x.ProductImages)
        //       .Include(x => x.ProductColors).ThenInclude(x => x.Color)
        //       .Include(x => x.Category).Include(x => x.ProductComments)
        //       .Include(x => x.Brand)
        //       .FirstOrDefault(x => x.Id == comment.ProductId);

        //    if (product == null) return NotFound();

        //    DetailDto detailDto = new DetailDto
        //    {
        //        Product = product,
        //        Comment = comment,
        //        RelatedProduct = _context.Products
        //        .Include(x => x.ProductImages).Include(x => x.Brand)
        //        .Where(x => x.CategoryId == product.CategoryId).OrderByDescending(x => x.Id).Take(5).ToList()
        //    };

        //    //if (!ModelState.IsValid)
        //    //{
        //    //    TempData["error"] = "Comment model is not valid!";

        //    //    return View("Detail", detailDto);
        //    //}

        //    if (!_context.Products.Any(x => x.Id == comment.ProductId))
        //    {
        //        TempData["error"] = "Selected Book not found";
        //        return View("Detail", detailDto);
        //    }

        //    if (!User.Identity.IsAuthenticated)
        //    {
        //        if (string.IsNullOrWhiteSpace(comment.Email))
        //        {
        //            TempData["error"] = "Email is required";
        //            return View("Detail", detailDto);
        //        }

        //        if (string.IsNullOrWhiteSpace(comment.FullName))
        //        {
        //            TempData["error"] = "FullName is required";
        //            return View("Detail", detailDto);
        //        }
        //    }
        //    else
        //    {
        //        AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);
        //        comment.AppUserId = user.Id;
        //        comment.FullName = user.FullName;
        //        comment.Email = user.Email;
        //    }

        //    comment.Status = false;
        //    comment.CreatedAt = DateTime.UtcNow.AddHours(4);
        //    _context.ProductComments.Add(comment);
        //    _context.SaveChanges();

        //    TempData["Success"] = "Comment added successfully";

        //    return RedirectToAction("detail", new { Id = comment.ProductId });
        //}

    }
}