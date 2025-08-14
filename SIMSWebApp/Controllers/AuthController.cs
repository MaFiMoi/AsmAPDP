using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SIMSWebApp.Controllers
{
    public class AuthController : Controller
    {
        [AllowAnonymous] // khong can dang nhap
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
