using Microsoft.AspNetCore.Mvc;

namespace OnlineEducationWebApp.Controllers
{
    public class LessonController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
