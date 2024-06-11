using OnlineEducationWebApp.Data.Entities;

namespace OnlineEducationWebApp.Interfaces
{
    public interface IDocumentService
    {
        public Task<Document> GetByIdAsync(int id);
        public Task<Document> CreateAsync(Document document, int id);
        public Task DeleteAsync(int id);
        public Task<List<Document>> GetDocumentForLessonAsync(int id);
        Task<Document> UploadAsync(Document document, IFormFile file);

    }
}
