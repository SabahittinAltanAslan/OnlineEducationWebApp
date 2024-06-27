using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;

namespace OnlineEducationWebApp.Controllers
{
    public class TeacherController : Controller
    {
        private readonly ITeacherService _service;

        public TeacherController(ITeacherService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllAsync();

            return View(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)//FromQuery
        {
            var result = await _service.GetByIdAsync(id);
            if (result == null)
            {
                return NotFound(id);
            }
            return View(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(Teacher teacher)//FromBody
        {
            var addedProduct = await _service.CreateAsync(teacher);
            return RedirectToAction("Index");
        }

        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(Teacher teacher)
        {
            var checkProduct = await _service.GetByIdAsync(teacher.Id);
            if (checkProduct == null)
            {
                return NotFound(teacher.Id);
            }
            await _service.UpdateAsync(teacher);
            return RedirectToAction("Index");
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var checkProduct = await _service.GetByIdAsync(id);
            if (checkProduct == null)
            {
                return NotFound(id);
            }
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}