using Microsoft.EntityFrameworkCore;
using OnlineEducationWebApp.Data.Context;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;

namespace OnlineEducationWebApp.Data.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ProjectContext _context;

        private readonly IWebHostEnvironment _environment;

        public DocumentService(ProjectContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        //public async Task<Document> CreateAsync(Document document,int lessonId)
        //{
        //    var lesson = await _context.Lessons.FindAsync(lessonId);
        //    if (lesson != null)
        //    {
        //        document.Lesson = lesson;
        //        await _context.Documents.AddAsync(document);
        //        await _context.SaveChangesAsync();
        //        return document;
        //    }
        //    return null;
        //}

        public async Task DeleteAsync(int id)
        {
            var removedEntity = await _context.Documents.FindAsync(id);
            _context.Documents.Remove(removedEntity);
            await _context.SaveChangesAsync();

            if (File.Exists(removedEntity.FilePath))
            {
                File.Delete(removedEntity.FilePath);
            }
        }

        public async Task<Document> GetByIdAsync(int id)
        {
            return await _context.Documents.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Document>> GetDocumentForLessonAsync(int lessonId)
        {
            return await _context.Documents
                .Where(document => document.LessonId == lessonId)
                .ToListAsync();
        }

        public async Task<Document> UploadAsync(Document document, IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or null");
            }

            var lesson = await _context.Lessons.FindAsync(document.LessonId);
            if (lesson == null)
            {
                throw new ArgumentException("Lesson not found");
            }

            string uniqueFileName = Guid.NewGuid().ToString() + "_" + file.FileName;
            string filePath = Path.Combine(_environment.ContentRootPath + "//Documents", uniqueFileName);

            using (var fileStream = new FileStream(filePath, FileMode.CreateNew))
            {
                await file.CopyToAsync(fileStream);
            }

            document.FileName = uniqueFileName;
            document.OriginalFileName = file.FileName;
            document.FilePath = filePath;
            document.Lesson = lesson;

            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();

            return document;
        }
    }
}