using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;
using OnlineEducationWebApp.Interfaces;
using OnlineEducationWebApp.Models;

namespace OnlineEducationWebApp.Controllers
{
    [Route("[controller]")]
    public class StudentLessonController : Controller
    {
        private readonly IStudentLessonService _service;

        public StudentLessonController(IStudentLessonService service)
        {
            _service = service;
        }

        [HttpPost("{lessonId}/kick-student")]
        [Authorize(Roles = UserRoles.Teacher)]
        public async Task<IActionResult> KickStudent([FromRoute] int lessonId, [FromForm] int studentId)
        {
            try
            {
                var checkProduct = await _service.GetByIdForKickAsync(lessonId, studentId);
                if (checkProduct == null)
                {
                    return NotFound();
                }

                await _service.KickStudentAsync(checkProduct);
                return RedirectToAction("GetStudentForLesson", "Student", new { id = lessonId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error");
            }
        }
    }
}