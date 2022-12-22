using BrandShop.Business.DTOs.ServiceDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BrandShopMVC.Areas.Manage.Controllers
{
    [Area("manage")]
    public class ServiceController : Controller
    {
        private readonly AppDbContext _context;

        public ServiceController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var services = await _context.Services.ToListAsync();
            return View(services);
        }

        public async Task<IActionResult> Info(int id)
        {
            Service service = await _context.Services.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (service == null) return NotFound();

            return View(service);
        }

        public async Task<IActionResult> Edit(int id)
        {
            Service service = await _context.Services.Where(x => x.Id == id).FirstOrDefaultAsync();

            if (service == null) return NotFound();

            UpdateServiceDto serviceDto = new UpdateServiceDto
            {
                Title = service.Title,
                Description = service.Description,
                Icon = service.Icon,
            };

            return View(serviceDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, UpdateServiceDto serviceDto)
        {
            Service existService = await _context.Services.Where(x => x.Id == id).FirstOrDefaultAsync();

            if(existService == null) return NotFound();

            if (!ModelState.IsValid) return View();

            Service service = new Service
            {
                Title = existService.Title,
                Description = existService.Description,
                Icon = existService.Icon,
            };

            existService.Title = serviceDto.Title;
            existService.Description = serviceDto.Description;
            existService.Icon = serviceDto.Icon;
            existService.UpdatedAt = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
