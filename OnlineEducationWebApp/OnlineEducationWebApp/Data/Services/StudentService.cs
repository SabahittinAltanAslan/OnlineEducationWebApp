using Microsoft.EntityFrameworkCore;
using OnlineEducationWebApp.Data.Context;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;

namespace OnlineEducationWebApp.Data.Services
{
    public class StudentService : IStudentService
    {
        private readonly ProjectContext _context;

        public StudentService(ProjectContext context)
        {
            _context = context;
        }
        public async Task<Student> CreateAsync(Student student)
        {
            await _context.Students.AddAsync(student);
            await _context.SaveChangesAsync();
            return student;
        }

        public async Task DeleteAsync(int id)
        {
            var removedEntity = await _context.Students.FindAsync(id);
            _context.Students.Remove(removedEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _context.Students.ToListAsync();
        }

        public async Task<Student> GetStudentByIdAsync(int id)
        {
            return await _context.Students.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task UpdateAsync(Student student)
        {
            var unchangedEntity = await _context.Students.FindAsync(student.Id);
            _context.Entry(unchangedEntity).CurrentValues.SetValues(student);
            await _context.SaveChangesAsync();
        }
    }
}
