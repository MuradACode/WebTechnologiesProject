using BrandShop.Business.DTOs.HomeDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BrandShopMVC.Controllers
{
    
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(AppDbContext context , UserManager<AppUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            if(User.Identity.IsAuthenticated)
            {
                AppUser member = await _userManager.FindByNameAsync(User.Identity.Name);
                HomeIndexDto home = new HomeIndexDto()
                {
                    Heroes = _context.Heroes.FirstOrDefault(),
                    Services = _context.Services.ToList(),
                    Discounteds = _context.Discounteds.FirstOrDefault(),
                    IsTrendProd = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsTrend).Where(d => d.IsDeleted == false).ToList(),
                    IsBestProd = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsBest).ToList(),
                    IsSmartProd = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsSmart).ToList(),
                    IsLiked = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsLiked).ToList(),
                    IsDiscProd = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsDiscounted).FirstOrDefault(),
                    Brands = _context.Brands.ToList(),

                    Question = new QuestionDto
                    {
                        FullName = member.FullName,
                        Email = member.Email
                    },
                };

                return View(home);
            }

            HomeIndexDto homeDto = new HomeIndexDto()
            {
                Heroes = _context.Heroes.FirstOrDefault(),
                Services = _context.Services.ToList(),
                Discounteds = _context.Discounteds.FirstOrDefault(),
                IsTrendProd = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsTrend).Where(d => d.IsDeleted == false).ToList(),
                IsBestProd = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsBest).ToList(),
                IsSmartProd = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsSmart).ToList(),
                IsLiked = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsLiked).ToList(),
                IsDiscProd = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsDiscounted).FirstOrDefault(),
                Brands = _context.Brands.ToList(),

            };

            return View(homeDto);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index(QuestionDto questiondto)
        {
            AppUser member = await _userManager.FindByNameAsync(User.Identity.Name);
            HomeIndexDto home = new HomeIndexDto()
            {
                Heroes = _context.Heroes.FirstOrDefault(),
                Services = _context.Services.ToList(),
                Discounteds = _context.Discounteds.FirstOrDefault(),
                IsTrendProd = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsTrend).Where(d => d.IsDeleted == false).ToList(),
                IsBestProd = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsBest).ToList(),
                IsSmartProd = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsSmart).ToList(),
                IsLiked = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsLiked).ToList(),
                IsDiscProd = _context.Products.Include(x => x.ProductImages).Include(x => x.Brand).Where(x => x.IsDiscounted).FirstOrDefault(),
                Brands = _context.Brands.ToList(),
                Question = new QuestionDto
                {
                    FullName = member.FullName,
                    Email = member.Email
                },
            };
            
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Sualınız göndərilmədi!";
                return View(home);
            }

            Question question = new Question
            {
                FullName = questiondto.FullName,
                Email = questiondto.Email,
                Text = questiondto.Text,
                

            };

            _context.Questions.Add(question);
            _context.SaveChanges();
            TempData["Success"] = "Sualınız uğurla göndərildi.";

            return RedirectToAction("Index", "Home");
        }
    }
}