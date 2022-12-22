using BrandShop.Business.DTOs.ProductDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrandShopMVC.Areas.Manage.Controllers
{
    [Area("manage")]
    public class CategoryController : Controller
    {
        private readonly AppDbContext _context;

        public CategoryController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var category = await _context.Categories.ToListAsync();

            return View(category);
        }

        public async Task<IActionResult> Info(int id)
        {
            Category category = await _context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if(category == null) return NotFound();

            return View(category);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateCategoryDto categoryDto)
        {
            var categories = await _context.Categories.Where(x => x.IsDeleted == false).ToListAsync();

            if (!ModelState.IsValid) return View();


            foreach (var item in categories)
            {
                if(categoryDto.Name == item.Name)
                {
                    ModelState.AddModelError("", "");
                    return View();
                }
            }

            Category category = new Category
            {
                Name = categoryDto.Name,
            };

            _context.Categories.Add(category);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Category category = await _context.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (category == null) return NotFound();

            UpdateCategoryDto categoryDto = new UpdateCategoryDto
            {
                Name = category.Name,
            };

            return View(categoryDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateCategoryDto categoryDto)
        {
            Category existCategory = await _context.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (existCategory == null) return NotFound();

            if (!ModelState.IsValid) return View();

            existCategory.Name = categoryDto.Name;
            existCategory.UpdatedAt = DateTime.UtcNow.AddHours(4);

            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Category category = await _context.Categories.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (category == null) return NotFound();

            category.IsDeleted = true;
            category.DeletedAt = DateTime.UtcNow.AddHours(4);

            _context.SaveChanges();

            return Ok();
        }
    }
}
