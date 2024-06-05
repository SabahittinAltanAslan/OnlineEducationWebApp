using Microsoft.EntityFrameworkCore;
using OnlineEducationWebApp.Data.Context;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;

namespace OnlineEducationWebApp.Data.Services
{
    public class DocumentService : IDocumentService
    {
        private readonly ProjectContext _context;

        public DocumentService(ProjectContext context)
        {
            _context = context;
        }
        public async Task<Document> CreateAsync(Document document)
        {
            await _context.Documents.AddAsync(document);
            await _context.SaveChangesAsync();
            return document;
        }

        public async Task DeleteAsync(int id)
        {
            var removedEntity = await _context.Documents.FindAsync(id);
            _context.Documents.Remove(removedEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Document>> GetAllAsync()
        {
            return await _context.Documents.ToListAsync();
        }

        public async Task<Document> GetByIdAsync(int id)
        {
            return await _context.Documents.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Document document)
        {
            var unchangedEntity = await _context.Documents.FindAsync(document.Id);
            _context.Entry(unchangedEntity).CurrentValues.SetValues(document);
            await _context.SaveChangesAsync();
        }
    }
}
