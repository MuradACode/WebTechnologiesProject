using BrandShop.Business.DTOs.AuthenticateDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using BrandShopMVC.Services;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews()
       .AddFluentValidation(options =>
       {
           // Validate child properties and root collection elements
           options.ImplicitlyValidateChildProperties = true;
           options.ImplicitlyValidateRootCollectionElements = true;
           // Automatic registration of validators in assembly
           options.RegisterValidatorsFromAssemblyContaining<SignUpDtoValidator>();
       });


builder.Services.AddDbContext<AppDbContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("Default"));
});

builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequiredUniqueChars = 0;
    opt.Password.RequireUppercase = true;
    opt.Password.RequireNonAlphanumeric = false;
    opt.User.RequireUniqueEmail = false;
}).AddDefaultTokenProviders().AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IEmailService, EmailService>();

builder.Services.AddAuthentication().AddFacebook(options =>
{
    options.AppId = "913849049281408";
    options.AppSecret = "e93e23a78f2d93720eb7f9e46289abb9";
});
builder.Services.AddAuthentication().AddGoogle(options =>
{
    options.ClientId = "109569879327-iap2v4uh1crmids1p7d5ud18d0fhdtc7.apps.googleusercontent.com";
    options.ClientSecret = "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";
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();


app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=dashboard}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
