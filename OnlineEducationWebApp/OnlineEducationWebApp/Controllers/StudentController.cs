using Microsoft.AspNetCore.Mvc;

namespace OnlineEducationWebApp.Controllers
{
    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
