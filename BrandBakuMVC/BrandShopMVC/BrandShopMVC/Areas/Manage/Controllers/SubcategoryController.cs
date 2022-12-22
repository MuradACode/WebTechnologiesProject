using BrandShop.Business.DTOs.ProductDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrandShopMVC.Areas.Manage.Controllers
{
    [Area("manage")]
    public class SubcategoryController : Controller
    {
        private readonly AppDbContext _context;

        public SubcategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var subcategories = await _context.Subcategories.Include(x => x.Category).ToListAsync();

            return View(subcategories);
        }

        public async Task<IActionResult> Info(int id)
        {
            Subcategory subcategory = await _context.Subcategories.Include(x => x.Category).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (subcategory == null) return NotFound();

            return View(subcategory);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Categories = await _context.Categories.Where(x => x.IsDeleted == false).ToListAsync();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateSubcategoryDto subcategoryDto)
        {
            ViewBag.Categories = await _context.Categories.Where(x => x.IsDeleted == false).ToListAsync();

            if (!ModelState.IsValid) return View();

            if (!_context.Categories.Any(x => x.Id == subcategoryDto.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category not found!");
                return View();
            }

            var subcategories = await _context.Subcategories.Where(x => x.IsDeleted == false).ToListAsync();

            foreach (var item in subcategories)
            {
                if(subcategoryDto.Name == item.Name && subcategoryDto.CategoryId == item.CategoryId)
                {
                    ModelState.AddModelError("", "Subcategory has already been created!");
                    return View();
                }
            }

            Subcategory subcategory = new Subcategory
            {
                Name = subcategoryDto.Name,
                CategoryId = subcategoryDto.CategoryId,
            };

            await _context.Subcategories.AddAsync(subcategory);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");

        }

        public async Task<IActionResult> Edit(int id)
        {
            ViewBag.Categories = await _context.Categories.Where(x => x.IsDeleted == false).ToListAsync();

            Subcategory existSubcategory = await _context.Subcategories.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (existSubcategory == null) return NotFound();

            UpdateSubcategoryDto subcategoryDto = new UpdateSubcategoryDto
            {
                Name = existSubcategory.Name,
                CategoryId = existSubcategory.CategoryId,
            };

            return View(subcategoryDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateSubcategoryDto subcategoryDto)
        {
            ViewBag.Categories = await _context.Categories.Where(x => x.IsDeleted == false).ToListAsync();

            Subcategory existSubcategory = await _context.Subcategories.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (existSubcategory == null) return NotFound();

            if (!ModelState.IsValid) return View();

            if (!_context.Categories.Any(x => x.Id == subcategoryDto.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category not found!");
                return View();
            }

            existSubcategory.Name = subcategoryDto.Name;
            existSubcategory.CategoryId = subcategoryDto.CategoryId;
            existSubcategory.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Subcategory subcategory = await _context.Subcategories.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (subcategory == null) return NotFound();

            subcategory.IsDeleted = true;
            subcategory.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
