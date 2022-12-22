using BrandShop.Business.DTOs.HomeDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using BrandShopMVC.Helper;
using Microsoft.AspNetCore.Mvc;

namespace BrandShopMVC.Areas.Manage.Controllers
{
    [Area("manage")]
    public class HeroController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public HeroController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }
        public IActionResult Index()
        {
            var hero = _context.Heroes.ToList();
            return View(hero);
        }

        public IActionResult Edit(int id)
        {
            //Hero hero = _context.Heroes.FirstOrDefault(x => x.Id == id);
            var hero = _context.Heroes.FirstOrDefault(x => x.Id == id);
            if (hero == null) return NotFound();

            UpdateHeroDto heroDto = new UpdateHeroDto()
            {
                Title = hero.Title,
                Description = hero.Description,
                BtnText = hero.BtnText,
                BtnLink = hero.BtnLink,
                Image = hero.Image,
                BackgroundImage = hero.BackgroundImage,
            };

            return View(heroDto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(int id, UpdateHeroDto heroDto)
        {
            Hero existHero = _context.Heroes.FirstOrDefault(x => x.Id == id);

            if (existHero == null) return NotFound();
            if (!ModelState.IsValid) return View();

            //var errorList = ModelState.ToDictionary(
            //    kvp => kvp.Key,
            //    kvp => kvp.Value.Errors.Select(e => e.ErrorMessage).ToArray()
            //);

            //return Json(errorList);

            if (heroDto.ImageFile != null)
            {
                if(heroDto.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Image max size is 2MB");
                    return View();
                }

                if(heroDto.ImageFile.ContentType != "image/png" && heroDto.ImageFile.ContentType != "image/jpeg" && heroDto.ImageFile.ContentType != "image/webp")
                {
                    ModelState.AddModelError("ImageFile", "Content type must be image/jpeg, image/png or image/webp!");
                    return View();
                }

                FileManager.Delete(_env.WebRootPath, "uploads/hero", existHero.Image);
                existHero.Image = FileManager.Save(_env.WebRootPath, "uploads/hero", heroDto.ImageFile);
            }

            if (heroDto.BgImageFile != null)
            {
                if (heroDto.BgImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("BgImageFile", "Image max size is 2MB");
                    return View();
                }

                if (heroDto.BgImageFile.ContentType != "image/png" && heroDto.BgImageFile.ContentType != "image/jpeg" && heroDto.BgImageFile.ContentType != "image/webp")
                {
                    ModelState.AddModelError("BgImageFile", "Content type must be image/jpeg, image/png or image/webp!");
                    return View();
                }

                FileManager.Delete(_env.WebRootPath, "uploads/hero", existHero.BackgroundImage);
                existHero.BackgroundImage = FileManager.Save(_env.WebRootPath, "uploads/hero", heroDto.BgImageFile);
            }

            existHero.Title = heroDto.Title;
            existHero.BtnText = heroDto.BtnText;
            existHero.BtnLink = heroDto.BtnLink;
            existHero.Description = heroDto.Description;
            existHero.UpdatedAt = DateTime.UtcNow.AddHours(4);

            _context.SaveChanges();       

            return RedirectToAction("Index", "Hero");
        }
    }
}
