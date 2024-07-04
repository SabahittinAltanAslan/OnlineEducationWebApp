using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;
using OnlineEducationWebApp.Models;

namespace OnlineEducationWebApp.Controllers
{
    [Route("[controller]")]
    public class StudentController : Controller
    {
        private readonly IStudentService _service;

        public StudentController(IStudentService service)
        {
            _service = service;
        }

        [HttpGet("GetStudentForLesson/{id}")]
        [Authorize(Roles = UserRoles.Teacher)]
        public async Task<IActionResult> GetStudentForLesson(int id)
        {
            var result = await _service.GetStudentForLessonAsync(id);
            ViewBag.LessonId = id;
            return View(result);
        }

        [HttpGet]
        [Authorize(Roles = UserRoles.Student)]
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
            return RedirectToAction("SignIn", "Home");
        }

        [HttpDelete]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            var checkProduct = await _service.GetStudentByIdAsync(id);
            if (checkProduct == null)
            {
                return NotFound(id);
            }
            await _service.DeleteAsync(id);
            return RedirectToAction("SignIn", "Home");//Login Sayfasına Yönlendirilecek Öğrenci Layoutunda Hesap sil butonu olacak orası için method
        }
    }
}