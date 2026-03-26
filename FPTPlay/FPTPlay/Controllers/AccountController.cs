using FPTPlay.Services;
using FPTPlay.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using FPTPlay.Helpers;
using FPTPlay.Models;

namespace FPTPlay.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserService _userService;
        private readonly IWebHostEnvironment _env;

        public AccountController(UserService userService, IWebHostEnvironment env)
        {
            _userService = userService;
            _env = env;
        }

        // ================= REGISTER =================
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existing = await _userService.GetByPhoneAsync(model.Mobile);
            if (existing != null)
            {
                ModelState.AddModelError("", "Số điện thoại đã tồn tại");
                return View(model);
            }

            var user = new User
            {
                Mobile = model.Mobile,
                FullName = model.FullName,
                PasswordHash = Hash.HashPassword(model.Password),
                Role = "User",
                IsActive = true,
                CreatedAt = DateTime.Now
            };

            await _userService.AddAsync(user);

            return RedirectToAction("Login");
        }

        // ================= LOGIN =================
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.LoginAsync(model.Mobile, model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Sai số điện thoại hoặc mật khẩu");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Mobile),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity),
                new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe
                });

            return RedirectToAction("Index", "Home");
        }

        // ================= LOGOUT =================
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        // ================= CHANGE PASSWORD =================
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordVM model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userService.GetCurrentUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            user.PasswordHash = Hash.HashPassword(model.NewPassword);

            await _userService.UpdateAsync(user);

            TempData["Success"] = "Đổi mật khẩu thành công!";
            return RedirectToAction("Index", "Home");
        }

        // ================= PROFILE =================
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var user = await _userService.GetCurrentUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            return View(user);
        }

        // ================= UPLOAD AVATAR =================
        [HttpPost]
        public async Task<IActionResult> UploadAvatar(IFormFile avatar)
        {
            var user = await _userService.GetCurrentUserAsync(User);
            if (user == null) return RedirectToAction("Login");

            if (avatar != null && avatar.Length > 0)
            {
                var folder = Path.Combine(_env.WebRootPath, "uploads");

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                var fileName = Guid.NewGuid() + Path.GetExtension(avatar.FileName);
                var path = Path.Combine(folder, fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await avatar.CopyToAsync(stream);
                }

                user.AvatarUrl = "/uploads/" + fileName;

                await _userService.UpdateAsync(user);
            }

            return RedirectToAction("Profile");
        }

        // ================= ACCESS DENIED =================
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}