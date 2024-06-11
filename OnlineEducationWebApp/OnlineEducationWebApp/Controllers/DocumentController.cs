using Microsoft.AspNetCore.Mvc;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Data.Services;
using OnlineEducationWebApp.Interfaces;

namespace OnlineEducationWebApp.Controllers
{
    public class DocumentController : Controller
    {
        private readonly IDocumentService _service;
        private readonly ILessonService _lessonService;
        public DocumentController(IDocumentService service, ILessonService lessonService)
        {
            _service = service;
            _lessonService = lessonService;
        }

        [HttpGet]
        public async Task<IActionResult> GetForLesson(int id)
        {
            var result = await _service.GetDocumentForLessonAsync(id);
            return View(result);

        }

        [HttpGet]
        public IActionResult Create(int lessonId)
        {
            var document = new Document { LessonId = lessonId };
            return View(document);
        }

        [HttpPost]
        public async Task<IActionResult> Create(Document document, IFormFile pdfFile)
        {
            if (ModelState.IsValid)
            {
                await _service.UploadAsync(document, pdfFile);
                return RedirectToAction("GetForLesson", new { lessonId = document.LessonId });
            }
            return View(document);
        }

        [HttpGet]
        public async Task<IActionResult> Download(int id)
        {
            var document = await _service.GetByIdAsync(id);
            if (document == null)
            {
                return NotFound();
            }

            byte[] fileBytes = System.IO.File.ReadAllBytes(document.FilePath);
            return File(fileBytes, "application/octet-stream", document.OriginalFileName);
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
