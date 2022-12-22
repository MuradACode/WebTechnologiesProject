using BrandShop.Business.DTOs.ProductDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrandShopMVC.Areas.Manage.Controllers
{
    [Area("manage")]
    public class ColorController : Controller
    {
        private readonly AppDbContext _context;

        public ColorController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var colors = await _context.Colors.ToListAsync();
            return View(colors);
        }

        public async Task<IActionResult> Info(int id)
        {
            Color color = await _context.Colors.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (color == null) return NotFound();

            return View(color);
        }

        public async Task<IActionResult> Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateColorDto colorDto)
        {
            if (!ModelState.IsValid) return View();

            var colors = await _context.Colors.ToListAsync();

            foreach (var item in colors)
            {
                if (item.ColorHexCode == colorDto.ColorHexCode || item.Name == colorDto.Name)
                {
                    ModelState.AddModelError("", "This color is already available!");
                    return View();
                }
            }

            Color color = new Color
            {
                Name = colorDto.Name,
                ColorHexCode = colorDto.ColorHexCode,
            };

            _context.Colors.Add(color);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int id)
        {
            Color color = await _context.Colors.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (color == null) return NotFound();

            UpdateColorDto colorDto = new UpdateColorDto
            {
                Name = color.Name,
                ColorHexCode = color.ColorHexCode,
            };

            return View(colorDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateColorDto colorDto)
        {
            Color existColor = await _context.Colors.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (existColor == null) return NotFound();

            if (!ModelState.IsValid) return View();

           

            existColor.Name = colorDto.Name;
            existColor.ColorHexCode = colorDto.ColorHexCode;
            existColor.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Delete(int id)
        {
            Color color = await _context.Colors.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (color == null) return NotFound();

            color.IsDeleted = true;
            color.DeletedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return Ok();
        }
    }
}
