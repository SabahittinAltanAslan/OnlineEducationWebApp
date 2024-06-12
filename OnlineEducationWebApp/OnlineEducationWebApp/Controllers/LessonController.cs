using Microsoft.AspNetCore.Mvc;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;

namespace OnlineEducationWebApp.Controllers
{
    public class LessonController : Controller
    {
        private readonly ILessonService _service;
        public LessonController(ILessonService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetForStudent()
        {
            var result = await _service.GetLessonsAsync(); 
            return View(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetForTeacher(int id)
        {
            var result = await _service.GetTeacherLessonAsync(id);
            return View(result);

        }

        //Ders üretirken hocanın Id değeri burada doldurulup post edilmeye yollanacak
        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Lesson lesson,int teacherId)//FromBody
        {
            teacherId = 1;
            var addedProduct = await _service.CreateAsync(lesson,teacherId);
            return RedirectToAction("GetForTeacher", new { id = teacherId });
        }


        [HttpDelete("{id}")]
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
    }
}
