using BrandShop.Business.DTOs.AuthenticateDto;
using BrandShop.Core.Entities;
using BrandShop.Data.DAL;
using BrandShopMVC.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace BrandShopMVC.Controllers
{
    public class AuthenticateController : Controller
    {

        private readonly AppDbContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;

        public AuthenticateController(AppDbContext context, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }

        public async Task<IActionResult> SignIn()
        {
            AppUser user = User.Identity.IsAuthenticated ? await _userManager.FindByNameAsync(User.Identity.Name) : null;

            if (user != null && user.IsAdmin == false)
            {
                return RedirectToAction("index", "home");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignIn(SignInDto signindto)
        {
            if (!ModelState.IsValid) return View();

            var existUser = await _userManager.Users.Where(x => x.Email == signindto.Email).FirstOrDefaultAsync();

            if(existUser == null)
            {
                ModelState.AddModelError("" , "Email or password is incorrect!");
                TempData["Error"] = "Giriş uğursuzdur.";
                return View();
            }

            if(!existUser.IsAdmin)
            {
                var result = await _signInManager.PasswordSignInAsync(existUser, signindto.Password, false, false);

                if(!result.Succeeded)
                {
                    ModelState.AddModelError("", "Email or password is incorrect!");
                    TempData["Error"] = "Giriş uğursuzdur.";
                    return View();
                }
            }
            TempData["Success"] = "Uğurla giriş etdiniz.";
            return RedirectToAction("index", "home");

        }

        public IActionResult SignUp()
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "home");
            }

            return View();
        }

        public IActionResult FacebookLogin(string returnUrl)
        {
            string redirectUrl = Url.Action("SocialMediaResponse", "Authenticate", new { returnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Facebook", redirectUrl);
            return new ChallengeResult("Facebook", properties);
        }

        public IActionResult GoogleLogin(string returnUrl)
        {
            string redirectUrl = Url.Action("SocialMediaResponse", "Authenticate", new { returnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        public async Task<IActionResult> SocialMediaResponse(string returnUrl)
        {
            var loginInfo = await _signInManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return RedirectToAction("SignUp");
            }
            else
            {
                var result =
                    await _signInManager.ExternalLoginSignInAsync(loginInfo.LoginProvider, loginInfo.ProviderKey, true);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    if (loginInfo.Principal.HasClaim(scl => scl.Type == ClaimTypes.Email))
                    {
                        AppUser user = new AppUser()
                        {
                            Email = loginInfo.Principal.FindFirstValue(ClaimTypes.Email),
                            FullName = loginInfo.Principal.FindFirstValue(ClaimTypes.Name),
                            UserName = loginInfo.Principal.FindFirstValue(ClaimTypes.Surname),
                            EmailConfirmed = true
                        };
                        var createResult = await _userManager.CreateAsync(user);
                        if (createResult.Succeeded)
                        {
                            var identityLogin = await _userManager.AddLoginAsync(user, loginInfo);
                            if (identityLogin.Succeeded)
                            {
                                await _signInManager.SignInAsync(user, true);
                                return Redirect("Signin");
                            }
                        }
                    }
                }
            }

            return RedirectToAction("SignUp");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SignUp(SignUpDto signupdto)
        {
            if (!ModelState.IsValid) return View();

            var existEmail = await _userManager.FindByEmailAsync(signupdto.Email);
            if(existEmail != null)
            {
                ModelState.AddModelError("Email", "Email is alread exist!");
                TempData["Error"] = "Qeydiyyat uğursuzdur.";
            }

            var existUsername = await _userManager.FindByNameAsync(signupdto.UserName);

            if(existUsername != null)
            {
                ModelState.AddModelError("Username", "Username is already exist!");
                TempData["Error"] = "Qeydiyyat uğursuzdur.";
            }

            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Qeydiyyat uğursuzdur.";
                return View();
            }

            AppUser newUser = new AppUser
            {
                FullName = signupdto.FullName,
                UserName = signupdto.UserName,
                Email = signupdto.Email,
                PhoneNumber = signupdto.PhoneNumber,
                Address = signupdto.Address,
                IsAdmin = false,
            };

            var result = await _userManager.CreateAsync(newUser, signupdto.Password);

            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
                TempData["Error"] = "Qeydiyyat uğursuzdur.";
                return View();
            }

            await _userManager.AddToRoleAsync(newUser, "Member");

            await _signInManager.PasswordSignInAsync(newUser, signupdto.Password, false, false);
            TempData["Success"] = "Qeydiyyat uğurludur.";
            return RedirectToAction("index", "home");


        }

        public async Task<IActionResult> Forgot()
        {
            if(User.Identity.IsAuthenticated)
            {
                return RedirectToAction("index", "home");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Forgot(ForgotDto forgotdto)
        {
            AppUser user = await _userManager.FindByEmailAsync(forgotdto.Email);

            if (!ModelState.IsValid)
            {
                return View();
            }

            if(user == null)
            {
                ModelState.AddModelError("Email", "User not found!");
                return View();
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var url = Url.Action("resetpassword" , "authenticate" , new {email = forgotdto.Email , token = token} , Request.Scheme);

            string body = string.Empty;

            using (StreamReader reader = new StreamReader("wwwroot/templateHtml/reset-password-email.html"))
            {
                body = reader.ReadToEnd();
            }
            body = body.Replace("{{url}}", url);


            _emailService.Send(forgotdto.Email, "ChangePassword", body);

            return RedirectToAction("SignIn", "Authenticate");
        }

        public async Task<IActionResult> ResetPassword(ResetDto resetdto)
        {
            if (resetdto.Email == null) return RedirectToAction("index", "error");

            AppUser user = await _userManager.FindByEmailAsync(resetdto.Email);
            if(user == null || !(await _userManager.VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", resetdto.Token)))
            {
                return RedirectToAction("SignIn", "Authenticate");
            }

            return View(resetdto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ResetDto resetdto)
        {
            if (!ModelState.IsValid) return View("ResetPassword" , resetdto);

            AppUser user = await _userManager.FindByEmailAsync(resetdto.Email);

            if (user == null) return RedirectToAction("SignIn", "Authenticate");

            var result = await _userManager.ResetPasswordAsync(user , resetdto.Token , resetdto.Password);

            if(!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                return RedirectToAction("ResetPassword", resetdto);
            }

            user.UpdatedAt = DateTime.UtcNow.AddHours(4);
            _context.SaveChanges();

            return RedirectToAction("SignIn", "Authenticate");

        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        //[Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile()
        {
            AppUser user = await _userManager.FindByNameAsync(User.Identity.Name);

            ProfileDto profile = new ProfileDto
            {
                Update = new UpdateDto { 
                    UserName = user.UserName, 
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    EmailConfirmed = user.EmailConfirmed,
                    Address = user.Address,
                    FullName = user.FullName
                },
            };
            return View(profile);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "Member")]
        public async Task<IActionResult> Profile(UpdateDto updateDto)
        {
            


            AppUser member = await _userManager.FindByNameAsync(User.Identity.Name);

            ProfileDto profileDto = new ProfileDto
            {
                Update = updateDto
            };

            //if (!ModelState.IsValid)
            //{
            //    return View(profileDto);
            //}

            if (member.Email != updateDto.Email && _userManager.Users.Any(x => x.NormalizedEmail == updateDto.Email.ToUpper()))
            {
                ModelState.AddModelError("Email", "This email has already been taken");
                return View(profileDto);
            }

            if (member.UserName != updateDto.UserName && _userManager.Users.Any(x => x.NormalizedUserName == updateDto.UserName.ToUpper()))
            {
                ModelState.AddModelError("UserName", "This username has already been taken");
                return View(profileDto);
            }

            if(member.PhoneNumber != updateDto.PhoneNumber && _userManager.Users.Any(x => x.PhoneNumber == updateDto.PhoneNumber))
            {
                ModelState.AddModelError("PhoneNumber", "This number has already been taken");
                return View(profileDto);
            }

            if (member.Address != updateDto.Address && _userManager.Users.Any(x => x.Address == updateDto.Address))
            {
                ModelState.AddModelError("Address", "This address has already been taken");
                return View(profileDto);
            }

            member.Email = updateDto.Email;
            member.UserName = updateDto.UserName;
            member.Address = updateDto.Address;
            member.PhoneNumber = updateDto.PhoneNumber;
            member.UpdatedAt = DateTime.UtcNow.AddHours(4);

            var result = await _userManager.UpdateAsync(member);

            if (!result.Succeeded)
            {
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError("", item.Description);
                }
                return View(profileDto);
            }

            if (!string.IsNullOrWhiteSpace(updateDto.Password) && !string.IsNullOrWhiteSpace(updateDto.RepeatPassword))
            {
                if (updateDto.Password != updateDto.RepeatPassword)
                {
                    return View(profileDto);
                }

                if (!await _userManager.CheckPasswordAsync(member, updateDto.CurrentPassword))
                {
                    ModelState.AddModelError("CurrentPassword", "CurrentPassword is not correct");
                    return View(profileDto);
                }

                var passwordResult = await _userManager.ChangePasswordAsync(member, updateDto.CurrentPassword, updateDto.Password);

                if (!passwordResult.Succeeded)
                {
                    foreach (var item in passwordResult.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                    return View(profileDto);
                }
            }
            _context.SaveChanges();

            await _signInManager.SignOutAsync();

            return RedirectToAction("Index" , "Home");
        }
    }
}
