using Microsoft.EntityFrameworkCore;
using OnlineEducationWebApp.Data.Context;
using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Interfaces;

namespace OnlineEducationWebApp.Data.Services
{
    public class StudentLessonService : IStudentLessonService
    {
        private readonly ProjectContext _context;

        public StudentLessonService(ProjectContext context)
        {
            _context = context;
        }

        public async Task<StudentLesson> GetByIdForKickAsync(int lessonId, int studentId)
        {
            return await _context.StudentLessons.AsNoTracking()
                .SingleOrDefaultAsync(x => x.LessonId == lessonId && x.StudentId == studentId);
        }

        public async Task KickStudentAsync(StudentLesson studentLesson)
        {
            var removedEntity = await _context.StudentLessons
                .SingleOrDefaultAsync(x => x.LessonId == studentLesson.LessonId && x.StudentId == studentLesson.StudentId);
            _context.StudentLessons.Remove(removedEntity);
            await _context.SaveChangesAsync();
        }
    }
}