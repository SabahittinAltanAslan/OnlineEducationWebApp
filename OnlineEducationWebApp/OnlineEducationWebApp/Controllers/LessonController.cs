using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using OnlineEducationWebApp.Data.Context;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;
using OnlineEducationWebApp.Models;

namespace OnlineEducationWebApp.Controllers
{
    [Route("[controller]")]
    public class LessonController : Controller
    {
        private readonly ILessonService _service;

        private readonly ProjectContext _context;

        public LessonController(ILessonService service, ProjectContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet("Index")]
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var result = await _service.GetLessonsAsync();
            return View(result);
        }

        [HttpGet("GetForTeacher/{id}")]
        [Authorize(Roles = UserRoles.Teacher)]
        public async Task<IActionResult> GetForTeacher([FromRoute] int id)
        {
            var result = await _service.GetTeacherLessonAsync(id);
            return View(result);
        }

        //Ders üretirken hocanın Id değeri burada doldurulup post edilmeye yollanacak
        [HttpGet]
        [Authorize(Roles = UserRoles.Teacher)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Teacher)]
        public async Task<IActionResult> Create(Lesson lesson)
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value, out int teacherId))
            {
                return BadRequest();
            }

            var addedProduct = await _service.CreateAsync(lesson, teacherId);
            return RedirectToAction("GetForTeacher", new { id = teacherId });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = UserRoles.Teacher)]
        public async Task<IActionResult> Delete(int id)
        {
            var checkProduct = await _service.GetLessonByIdAsync(id);
            if (checkProduct == null)
            {
                return NotFound(id);
            }
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }

        [HttpGet("join/{lessonId}")]
        [Authorize]
        public IActionResult Join([FromRoute] string lessonId)
        {
            var lesson = _context.Lessons.Find(lessonId);
            if (lesson == null)
            {
                return NotFound();
            }
            ViewBag.LessonId = lessonId;
            return View();
        }
    }
}