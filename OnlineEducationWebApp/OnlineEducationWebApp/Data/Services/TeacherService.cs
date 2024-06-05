using Microsoft.EntityFrameworkCore;
using OnlineEducationWebApp.Data.Context;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;

namespace OnlineEducationWebApp.Data.Services
{
    public class TeacherService : ITeacherService
    {
        private readonly ProjectContext _context;

        public TeacherService(ProjectContext context)
        {
            _context = context;
        }
        public async Task<Teacher> CreateAsync(Teacher teacher)
        {
            await _context.Teachers.AddAsync(teacher);
            await _context.SaveChangesAsync();
            return teacher;
        }

        public async Task DeleteAsync(int id)
        {
            var removedEntity = await _context.Teachers.FindAsync(id);
            _context.Teachers.Remove(removedEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Teacher>> GetAllAsync()
        {
            return await _context.Teachers.ToListAsync();
        }

        public async Task<Teacher> GetByIdAsync(int id)
        {
            return await _context.Teachers.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Teacher teacher)
        {
            var unchangedEntity = await _context.Teachers.FindAsync(teacher.Id);
            _context.Entry(unchangedEntity).CurrentValues.SetValues(teacher);
            await _context.SaveChangesAsync();
        }
    }
}
