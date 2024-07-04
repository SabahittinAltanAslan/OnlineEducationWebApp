using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using OnlineEducationWebApp.Data.Context;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;
using OnlineEducationWebApp.Models;
using OnlineEducationWebApp.Models.DTO;
using System.Security.Claims;

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

        [HttpGet("/lessons")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();
            return View(result);
        }

        [HttpGet("/my-lessons")]
        [Authorize]
        public async Task<IActionResult> MyLessons()
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value, out int userId))
            {
                return BadRequest();
            }

            List<Lesson> lessons = new List<Lesson>();

            // get user role
            var userRole = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value;
            if (userRole == UserRoles.Teacher)
            {
                lessons = await _service.GetTeacherLessonAsync(userId);
            }
            else if (userRole == UserRoles.Student)
            {
                lessons = await _service.GetStudentLessonAsync(userId);
            }

            return View(lessons);
        }

        [HttpGet("create")]
        [Authorize(Roles = UserRoles.Teacher)]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        [Authorize(Roles = UserRoles.Teacher)]
        public async Task<IActionResult> Create(LessonCreateRequest lesson)
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value, out int teacherId))
            {
                return BadRequest();
            }

            var addedProduct = await _service.CreateAsync(lesson, teacherId);
            return RedirectToAction("MyLessons");
        }

        [HttpGet("{lessonUrl}/class")]
        [Authorize]
        public async Task<IActionResult> Class([FromRoute] string lessonUrl)
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value, out int userId))
            {
                return BadRequest();
            }

            var lesson = await _service.JoinLesson(lessonUrl, userId);
            if (lesson == null)
            {
                return NotFound();
            }

            return View(lesson);
        }

        [HttpGet("{lessonUrl}/start")]
        [Authorize(Roles = UserRoles.Teacher)]
        public async Task<IActionResult> Start([FromRoute] string lessonUrl)
        {
            var result = await _service.StartLesson(lessonUrl);
            if (result == null)
            {
                return NotFound();
            }

            return RedirectToAction("Class", new { lessonUrl });
        }

        // finish the lesson
        [HttpGet("{lessonId:int}/finish")]
        [Authorize(Roles = UserRoles.Teacher)]
        public async Task<IActionResult> Finish([FromRoute] int lessonId)
        {
            var result = await _service.FinishLesson(lessonId);
            if (result == null)
            {
                return NotFound();
            }

            return RedirectToAction("MyLessons");
        }

        [HttpGet("{lessonUrl}/join")]
        [Authorize(Roles = UserRoles.Student)]
        public async Task<IActionResult> Join([FromRoute] string lessonUrl)
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value, out int userId))
            {
                return BadRequest();
            }

            var result = await _service.JoinLesson(lessonUrl, userId);
            if (result == null)
            {
                return NotFound();
            }

            return RedirectToAction("Class", new { lessonUrl });
        }

        [HttpGet("subscribe/{lessonId}")]
        [Authorize(Roles = UserRoles.Student)]
        public async Task<IActionResult> Subscribe([FromRoute] int lessonId)
        {
            if (!int.TryParse(User.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti).Value, out int studentId))
            {
                return BadRequest();
            }

            await _service.Subscribe(studentId, lessonId);
            return RedirectToAction("MyLessons");
        }

        [HttpDelete("delete/{Id}")]
        [Authorize(Roles = UserRoles.Teacher)]
        public async Task<IActionResult> Delete(int Id)
        {
            var checkLesson = await _service.GetLessonByIdAsync(Id);
            if (checkLesson == null)
            {
                return NotFound();
            }
            await _service.DeleteAsync(Id);
            return RedirectToAction("MyLessons");
        }
    }
}