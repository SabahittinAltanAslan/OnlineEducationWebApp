using Microsoft.AspNetCore.Mvc;
using OnlineEducationWebApp.Interfaces;

namespace OnlineEducationWebApp.Controllers
{
    public class StudentLessonController : Controller
    {
        private readonly IStudentLessonService _service;
        public StudentLessonController(IStudentLessonService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> KickStudent(int lessonId, int studentId)
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


        [HttpPost]
        public async Task<IActionResult> SubscribeToLesson(int studentId, int lessonId)
        {
            await _service.SubscribeToLessonAsync(studentId, lessonId);
            return RedirectToAction("GetForStudent","Lesson", new { id = lessonId });
        }

        [HttpGet]
        public async Task<IActionResult> GetLessonsForStudent(int studentId)
        {//SELMAN STUDENT ID Yİ LOGİNDEN SONRA VİEWBAG İLE BURAYA DA VERMEN GERKİYOR.
            studentId = 1;
            var lessons = await _service.GetLessonsForStudentAsync(studentId);
            return View(lessons);
        }
    }
}
