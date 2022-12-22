using BrandShop.Business.DTOs.ProductDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using BrandShopMVC.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrandShopMVC.Areas.Manage.Controllers
{
    [Area("manage")]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public ProductController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.Include(x => x.Brand).Include(x => x.ProductImages).Include(x => x.ProductComments).Include(x => x.ProductColors).ThenInclude(x => x.Color).ToListAsync();
            return View(products);
        }

        public async Task<IActionResult> Info(int id)
        {
            Product product = _context.Products.Include(x => x.Brand).Include(x => x.Category).Include(x => x.ProductImages).Include(x => x.ProductColors).ThenInclude(x => x.Color).FirstOrDefault(x => x.Id == id);

            if (product == null) return NotFound();



            return View(product);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Colors = _context.Colors.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Brands = _context.Brands.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Categories = _context.Categories.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Subcategories = _context.Subcategories.Where(x => x.IsDeleted == false).ToList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductDto productDto)
        {
            ViewBag.Brands = _context.Brands.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Categories = _context.Categories.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Colors = _context.Colors.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Subcategories = _context.Subcategories.Where(x => x.IsDeleted == false).ToList();

            string[] fileTypes = { "image/jpeg", "image/png", "image/webp" };

            //if (!ModelState.IsValid) return View();

            //var errorList = ModelState.ToDictionary(
            //    kvp => kvp.Key,
            //    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            //);

            //return Json(errorList);

            Product product = new Product
            {
                BrandId = productDto.BrandId,
                SubcategoryId = productDto.SubcategoryId,
                Model = productDto.Model,
                Description = productDto.Description,
                CostPrice = productDto.CostPrice,
                SalePrice = productDto.SalePrice,
                DiscountPercent = productDto.DiscountPercent,
                StockStatus = productDto.StockStatus,
                IsTrend = productDto.IsTrend,
                IsBest = productDto.IsBest,
                IsSmart = productDto.IsSmart,
                IsRelated = productDto.IsRelated,
                IsDiscounted = productDto.IsDiscounted,
                IsLiked = productDto.IsLiked,
                CategoryId = productDto.CategoryId,
                //ProductImages = productDto.ProductImages,
            };

            if (!_context.Categories.Any(x => x.Id == productDto.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category not found!");
                return View();
            }

            if (!_context.Brands.Any(x => x.Id == productDto.BrandId))
            {
                ModelState.AddModelError("BrandId", "Brand not found!");
                return View();
            }

            if (!_context.Subcategories.Any(x => x.Id == productDto.SubcategoryId))
            {
                ModelState.AddModelError("SubcategoryId", "Brand not found!");
                return View();
            }

            if (productDto.PosterFile == null)
            {
                ModelState.AddModelError("PosterFile", "PosterFile is required!");
                return View();
            }

            if (productDto.PosterFile.Length > 2097152)
            {
                ModelState.AddModelError("PosterFile", "PosterFile max size is 2MB");
                return View();
            }

            if (!fileTypes.Contains(productDto.PosterFile.ContentType))
            {
                ModelState.AddModelError("PosterFile", "Content type must be png, jpeg, jpg or webp!");
                return View();
            }

            if (productDto.ImageFiles == null)
            {
                ModelState.AddModelError("ImageFiles", "Imagefiles is required!");
                return View();
            }

            var productImages = new List<ProductImage>();

            foreach (var item in productDto.ImageFiles)
            {
                if (item.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFiles", "Imagefiles max size is 2MB!");
                    return View();
                }

                if (!fileTypes.Contains(item.ContentType))
                {
                    ModelState.AddModelError("ImageFiles", "Content type must be png, jpeg, jpg or webp!");
                    return View();
                }

                ProductImage productImage = new ProductImage
                {
                    Image = FileManager.Save(_env.WebRootPath, "uploads/products", item),
                    PosterStatus = false,
                };

                productImages.Add(productImage);
            }

            _context.Products.Add(product);
            _context.SaveChanges();

            productImages.ForEach(productImg => productImg.ProductId = product.Id);
            _context.ProductImages.AddRange(productImages);

            ProductImage posterImage = new ProductImage
            {
                ProductId = product.Id,
                Image = FileManager.Save(_env.WebRootPath, "uploads/products", productDto.PosterFile),
                PosterStatus = true,
            };

            product.ProductImages.Add(posterImage);

            if (productDto.ColorIds != null)
            {
                foreach (var colorId in productDto.ColorIds)
                {
                    ProductColor productColor = new ProductColor
                    {
                        ProductId = product.Id,
                        ColorId = colorId,
                    };

                    _context.ProductColors.Add(productColor);
                }
            }
            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Brands = _context.Brands.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Categories = _context.Categories.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Colors = _context.Colors.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Subcategories = _context.Subcategories.Where(x => x.IsDeleted == false).ToList();

            Product product = _context.Products.Include(x => x.Brand).Include(x => x.Category).Include(x => x.ProductImages).Include(x => x.ProductColors).ThenInclude(x => x.Color).FirstOrDefault(x => x.Id == id);

            if (product == null) return NotFound();


            UpdateProductDto productDto = new UpdateProductDto
            {
                BrandId = product.BrandId,
                SubcategoryId = product.SubcategoryId,
                Model = product.Model,
                Description = product.Description,
                CostPrice = product.CostPrice,
                SalePrice = product.SalePrice,
                DiscountPercent = product.DiscountPercent,
                StockStatus = product.StockStatus,
                IsTrend = product.IsTrend,
                IsBest = product.IsBest,
                IsSmart = product.IsSmart,
                IsRelated = product.IsRelated,
                IsDiscounted = product.IsDiscounted,
                IsLiked = product.IsLiked,
                CategoryId = product.CategoryId,
                ColorIds = product.ProductColors.Select(x => x.ColorId).ToList()

                //ProductImages = productDto.ProductImages,
            };



            return View(productDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateProductDto productDto)
        {
            ViewBag.Brands = _context.Brands.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Categories = _context.Categories.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Colors = _context.Colors.Where(x => x.IsDeleted == false).ToList();
            ViewBag.Subcategories = _context.Subcategories.Where(x => x.IsDeleted == false).ToList();



            Product existProduct = _context.Products.Include(x => x.Brand).Include(x => x.Category).Include(x => x.ProductImages).Include(x => x.ProductColors).ThenInclude(x => x.Color).FirstOrDefault(x => x.Id == id);

            if (existProduct == null) return NotFound();

            if (!ModelState.IsValid) return View();

            //var errorList = ModelState.ToDictionary(
            //    kvp => kvp.Key,
            //    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            //);

            //return Json(errorList);

            string[] filetypes = { "image/jpeg", "image/png", "image/webp" };

            Product product = new Product
            {
                BrandId = productDto.BrandId,
                SubcategoryId = productDto.SubcategoryId,
                Model = productDto.Model,
                Description = productDto.Description,
                CostPrice = productDto.CostPrice,
                SalePrice = productDto.SalePrice,
                DiscountPercent = productDto.DiscountPercent,
                StockStatus = productDto.StockStatus,
                IsTrend = productDto.IsTrend,
                IsBest = productDto.IsBest,
                IsSmart = productDto.IsSmart,
                IsRelated = productDto.IsRelated,
                IsDiscounted = productDto.IsDiscounted,
                IsLiked = productDto.IsLiked,
                CategoryId = productDto.CategoryId,
                ColorIds = productDto.ColorIds
                //ProductImages = productDto.ProductImages,
            };

            if (!_context.Categories.Any(x => x.Id == productDto.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category not found!");
                return View();
            }

            if (!_context.Brands.Any(x => x.Id == productDto.BrandId))
            {
                ModelState.AddModelError("BrandId", "Brand not found!");
                return View();
            }

            if (!_context.Subcategories.Any(x => x.Id == productDto.SubcategoryId))
            {
                ModelState.AddModelError("SubcategoryId", "Subcategory not found!");
                return View();
            }





            if (productDto.PosterFile != null)
            {
                ProductImage currentImage = existProduct.ProductImages.FirstOrDefault(x => x.PosterStatus == true);

                if (currentImage == null) return NotFound();

                if (productDto.PosterFile.Length > 2097152)
                {
                    ModelState.AddModelError("PosterFile", "Posterfile max size is 2MB");
                    return View();
                }

                if (!filetypes.Contains(productDto.PosterFile.ContentType))
                {
                    ModelState.AddModelError("PosterFile", "Content type must be png, jpeg, jpg or webp!");
                    return View();
                }

                _setProductImage(currentImage, productDto.PosterFile);
            }

            if (productDto.ImageFiles != null)
            {
                foreach (var imageItem in existProduct.ProductImages.Where(x => x.PosterStatus == false))
                {
                    FileManager.Delete(_env.WebRootPath, "uploads/products", imageItem.Image);
                    _context.ProductImages.Remove(imageItem);
                }

                foreach (var imageFile in productDto.ImageFiles)
                {
                    ProductImage productImage = new ProductImage
                    {
                        ProductId = product.Id,
                        PosterStatus = null,
                        Image = FileManager.Save(_env.WebRootPath, "uploads/products", imageFile)
                    };

                    _context.ProductImages.Add(productImage);
                    //existBook.BookImages.Add(bookImage);
                }
            }

            foreach (var colorId in product.ColorIds.Where(x => !existProduct.ProductColors.Any(bt => bt.ColorId == x)))
            {
                ProductColor productColor = new ProductColor
                {
                    ProductId = product.Id,
                    ColorId = colorId
                };

                existProduct.ProductColors.Add(productColor);
            }

            existProduct.UpdatedAt = DateTime.UtcNow.AddHours(4);

            _setProductData(existProduct, productDto);

            _context.SaveChanges();

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Delete(int id)
        {
            Product product = _context.Products.Where(x => x.Id == id).FirstOrDefault();

            if (product == null) return View();

            product.IsDeleted = true;
            product.DeletedAt = DateTime.UtcNow.AddHours(4);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }


        private void _setProductImage(ProductImage image, IFormFile file)
        {
            string newFileName = FileManager.Save(_env.WebRootPath, "uploads/products", file);

            FileManager.Delete(_env.WebRootPath, "uploads/products", image.Image);
            image.Image = newFileName;
        }

        private void _setProductData(Product existProduct, UpdateProductDto product)
        {
            existProduct.BrandId = product.BrandId;
            existProduct.SubcategoryId = product.SubcategoryId;
            existProduct.CategoryId = product.CategoryId;
            existProduct.Model = product.Model;
            existProduct.CostPrice = product.CostPrice;
            existProduct.SalePrice = product.SalePrice;
            existProduct.DiscountPercent = product.DiscountPercent;
            existProduct.Description = product.Description;
            existProduct.IsBest = product.IsBest;
            existProduct.IsTrend = product.IsTrend;
            existProduct.IsRelated = product.IsRelated;
            existProduct.IsDiscounted = product.IsDiscounted;
            existProduct.StockStatus = product.StockStatus;
            existProduct.IsLiked = product.IsLiked;

        }
    }
}
