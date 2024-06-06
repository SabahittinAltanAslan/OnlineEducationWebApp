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
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllStudentsAsync();

            return View(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)//FromQuery
        {
            var result = await _service.GetStudentByIdAsync(id);
            if (result == null)
            {
                return NotFound(id);
            }
            return View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Student student)//FromBody
        {
            var addedProduct = await _service.CreateAsync(student);
            return RedirectToAction("Index");
        }

        [HttpPut]
        public async Task<IActionResult> Update(Student student)
        {
            var checkProduct = await _service.GetStudentByIdAsync(student.Id);
            if (checkProduct == null)
            {
                return NotFound(student.Id);
            }
            await _service.UpdateAsync(student);
            return RedirectToAction("Index");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var checkProduct = await _service.GetStudentByIdAsync(id);
            if (checkProduct == null)
            {
                return NotFound(id);
            }
            await _service.DeleteAsync(id);
            return RedirectToAction("Index");
        }
    }
}
