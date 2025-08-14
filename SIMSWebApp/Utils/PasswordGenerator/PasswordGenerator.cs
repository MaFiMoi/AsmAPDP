using Microsoft.AspNetCore.Mvc;

namespace SIMSWebApp.Utils.PasswordGenerator
{
    public class PasswordGenerator : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private string GenerateRandomPassword(int length = 8)
        {
            const string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}
