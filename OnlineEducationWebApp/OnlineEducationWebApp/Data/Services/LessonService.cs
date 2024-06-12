using Microsoft.EntityFrameworkCore;
using OnlineEducationWebApp.Data.Context;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;

namespace OnlineEducationWebApp.Data.Services
{
    public class LessonService : ILessonService
    {
        private readonly ProjectContext _context;

        public LessonService(ProjectContext context)
        {
            _context = context;
        }
        public async Task<Lesson> CreateAsync(Lesson lesson, int teacherId)
        {
            var teacher = await _context.Teachers.FindAsync(teacherId);
            if (teacher != null)
            {
                lesson.Teacher = teacher;
                await _context.Lessons.AddAsync(lesson);
                await _context.SaveChangesAsync();
                return lesson;
            }
            return null;
        }

        public async Task DeleteAsync(int id)
        {
            var removedEntity = await _context.Lessons.FindAsync(id);
            _context.Lessons.Remove(removedEntity);
            await _context.SaveChangesAsync();
        }


        public async Task<Lesson> GetLessonByIdAsync(int id)
        {
            return await _context.Lessons.AsNoTracking().SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<Lesson>> GetLessonsAsync()
        {
            return await _context.Lessons.ToListAsync();
        }

        public async Task<List<Lesson>> GetTeacherLessonAsync(int teacherId)
        {
            return await _context.Lessons
                .Where(lesson => lesson.TeacherId == teacherId)
                .ToListAsync();
        }

    }
}
