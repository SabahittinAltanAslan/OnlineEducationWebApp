using Microsoft.AspNetCore.Mvc;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;


namespace OnlineEducationWebApp.Controllers
{
    public class StudentController : Controller
    {
        private readonly IStudentService _service;
        public StudentController(IStudentService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentForLesson(int id)
        {
            var result = await _service.GetStudentForLessonAsync(id);
            ViewBag.LessonId = id;
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetStudentById(int id)
        {
            var result = await _service.GetStudentByIdAsync(id);
            if (result == null)
            {
                return NotFound(id);
            }
            return View(result);
        }

        [HttpGet]
        public IActionResult CreateStudent()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateStudent(Student student)
        {
            var addedProduct = await _service.CreateAsync(student);
            return RedirectToAction("#");//Login sayfasına yönlendirilecek
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var checkProduct = await _service.GetStudentByIdAsync(id);
            if (checkProduct == null)
            {
                return NotFound(id);
            }
            await _service.DeleteAsync(id);
            return RedirectToAction("#");//Login Sayfasına Yönlendirilecek Öğrenci Layoutunda Hesap sil butonu olacak orası için method
        }
    }
}

