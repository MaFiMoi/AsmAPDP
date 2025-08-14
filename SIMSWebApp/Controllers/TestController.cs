using Microsoft.AspNetCore.Mvc;

namespace SIMSWebApp.Controllers
{
    public class TestController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public string Hello(int id, string name, int age)
        {
            return "Hello word - ASP.Net Core MVC, ID = "+ id + ", NAME = " + name + " , AGE = " + age;
            // host:port/test/hello => chay tren URL trinh duyet
            // host:port/test/hello?id=10&name=trieunt&age=20
        }
    }
}
