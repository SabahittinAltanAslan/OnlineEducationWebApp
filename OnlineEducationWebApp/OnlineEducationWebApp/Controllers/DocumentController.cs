using Microsoft.AspNetCore.Mvc;

namespace OnlineEducationWebApp.Controllers
{
    public class DocumentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
