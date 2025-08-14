using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SIMSWebApp.Models;
using SIMSWebApp.Services;
using System.Security.Claims;

namespace SIMSWebApp.Controllers
{
    [Authorize] // muon truy cap vao controller nay phai dang nhap
    public class LoginController : Controller
    {
        private readonly UserService _userService;
        public LoginController(UserService service)
        {
            _userService = service;
        }

        [HttpGet]
        [AllowAnonymous] // khong phai dang nhap
        public IActionResult Index()
        {
            // neu da dang nhap roi, thi khong cho dang nhap lai
            if (User.Identity?.IsAuthenticated == true)
            {
                return RedirectToAction("Index", "Dashboard");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous] // khong bat dang nhap
        public async Task<IActionResult> Index(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                // khong co loi xay ra
                string username = model.Username.Trim();
                string password = model.Password.Trim();
                var user = await _userService.LoginUserAsync(username, password);
                if (user == null)
                {
                    // thong bao thong tin sai tai khoan ra ngoai view
                    ViewData["InvalidAccount"] = "Your account invalid";
                    return View(model);
                }
                // luu thong tin nguoi dung vao cookies
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Username),
                    new Claim(ClaimTypes.Role, user.Role)
                };
                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                return RedirectToAction("Index", "Dashboard");
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }
            return RedirectToAction("Index", "Login");
        }


    }
}
