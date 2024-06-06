using Microsoft.AspNetCore.Mvc;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;

namespace OnlineEducationWebApp.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService _service;
        public DocumentController(IDocumentService service)
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
        public async Task<IActionResult> Create(Document document)//FromBody
        {
            var addedProduct = await _service.CreateAsync(document);
            return RedirectToAction("Index");
        }

        [HttpPut]
        public async Task<IActionResult> Update(Document document)
        {
            var checkProduct = await _service.GetByIdAsync(document.Id);
            if (checkProduct == null)
            {
                return NotFound(document.Id);
            }
            await _service.UpdateAsync(document);
            return RedirectToAction("Index");
        }

        [HttpDelete("{id}")]
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
