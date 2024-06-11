using Microsoft.AspNetCore.Mvc;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;


namespace OnlineEducationWebApp.Controllers
{
    [Route("students")]
    public class StudentController : Controller
    {
        private readonly IStudentService _service;
        public StudentController(IStudentService service)
        {
            _service = service;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _service.GetAllStudentsAsync();
            return View(result);
        }

        [HttpGet("get/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _service.GetStudentByIdAsync(id);
            if (result == null)
            {
                return NotFound(id);
            }
            return View(result);
        }

        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(Student student)
        {
            var addedProduct = await _service.CreateAsync(student);
            return RedirectToAction("GetAll");
        }

        [HttpGet("update/{id}")]
        public async Task<IActionResult> Update(int id)
        {
            var student = await _service.GetStudentByIdAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        [HttpPost("update/{id}")]
        public async Task<IActionResult> Update(Student student)
        {
            var checkProduct = await _service.GetStudentByIdAsync(student.Id);
            if (checkProduct == null)
            {
                return NotFound(student.Id);
            }
            await _service.UpdateAsync(student);
            return RedirectToAction("GetAll");
        }

        [HttpDelete("delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var checkProduct = await _service.GetStudentByIdAsync(id);
            if (checkProduct == null)
            {
                return NotFound(id);
            }
            await _service.DeleteAsync(id);
            return Redirect("/students/all");
        }
    }
}

