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
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetLessonsAsync();

            return View(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)//FromQuery
        {
            var result = await _service.GetLessonByIdAsync(id);
            if (result == null)
            {
                return NotFound(id);
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Lesson lesson)//FromBody
        {
            var addedProduct = await _service.CreateAsync(lesson);
            return RedirectToAction("Index");
        }

        [HttpPut]
        public async Task<IActionResult> Update(Lesson lesson)
        {
            var checkProduct = await _service.GetLessonByIdAsync(lesson.Id);
            if (checkProduct == null)
            {
                return NotFound(lesson.Id);
            }
            await _service.UpdateAsync(lesson);
            return RedirectToAction("Index");
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
