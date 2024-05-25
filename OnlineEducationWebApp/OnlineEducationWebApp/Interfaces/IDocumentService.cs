using OnlineEducationWebApp.Data.Entities;

namespace OnlineEducationWebApp.Interfaces
{
    public interface IDocumentService
    {
        public Task<List<Document>> GetAllAsync();
        public Task<Document> GetByIdAsync(int id);
        public Task<Document> CreateAsync(Document document);
        public Task UpdateAsync(Document document);
        public Task DeleteAsync(int id);
    }
}
