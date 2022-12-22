using BrandShop.Business.DTOs.ProductDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using BrandShopMVC.Helper;
using Microsoft.AspNetCore.Mvc;

namespace BrandShopMVC.Areas.Manage.Controllers
{
    [Area("manage")]

    public class BrandController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public BrandController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var brands = _context.Brands.ToList();
            return View(brands);
        }

        public async Task<IActionResult> Info(int id)
        {
            var brand = _context.Brands.FirstOrDefault(b => b.Id == id);
            return View(brand);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateBrandDto brandDto)
        {
            if (!ModelState.IsValid) return View();

            if (brandDto.ImageFile == null)
            {
                ModelState.AddModelError("ImageFile", "ImageFile is required!");
                return View();
            }

            if (brandDto.ImageFile != null)
            {
                if (brandDto.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Image max size is 2MB");
                    return View();
                }

                if (brandDto.ImageFile.ContentType != "image/png" && brandDto.ImageFile.ContentType != "image/jpeg" && brandDto.ImageFile.ContentType != "image/webp")
                {
                    ModelState.AddModelError("ImageFile", "Content type must be image/jpeg, image/png or image/webp!");
                    return View();
                }

                brandDto.Image = FileManager.Save(_env.WebRootPath, "uploads/brands", brandDto.ImageFile);
            }

            Brand brand = new Brand
            {
                Name = brandDto.Name,
                Link = brandDto.Link,
                Image = brandDto.Image,
            };

            _context.Brands.Add(brand);
            _context.SaveChanges();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Brand brand = _context.Brands.FirstOrDefault(b => b.Id == id);

            if (brand == null) return NotFound();

            UpdateBrandDto updateDto = new UpdateBrandDto
            {
                Name = brand.Name,
                Link = brand.Link,
                Image = brand.Image,
            };

            return View(updateDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateBrandDto brandDto)
        {
            Brand existBrand = _context.Brands.Where(x => x.Id == id).FirstOrDefault();

            if (existBrand == null) return NotFound();

            if (!ModelState.IsValid) return View();

            if (brandDto.ImageFile != null)
            {
                if (brandDto.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Image max size is 2MB");
                    return View();
                }

                if (brandDto.ImageFile.ContentType != "image/png" && brandDto.ImageFile.ContentType != "image/jpeg" && brandDto.ImageFile.ContentType != "image/webp")
                {
                    ModelState.AddModelError("ImageFile", "Content type must be image/jpeg, image/png or image/webp!");
                    return View();
                }

                FileManager.Delete(_env.WebRootPath, "uploads/hero", existBrand.Image);
                existBrand.Image = FileManager.Save(_env.WebRootPath, "uploads/hero", brandDto.ImageFile);
            }

            existBrand.Name = brandDto.Name;
            existBrand.Link = brandDto.Link;
            existBrand.UpdatedAt = DateTime.UtcNow.AddHours(4);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Brand brand = _context.Brands.Where(x => x.Id == id).FirstOrDefault();

            if (brand == null) return NotFound();

            brand.IsDeleted = true;
            brand.DeletedAt = DateTime.UtcNow.AddHours(4);
            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
