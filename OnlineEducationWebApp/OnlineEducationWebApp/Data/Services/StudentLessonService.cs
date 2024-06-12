using Microsoft.Build.Evaluation;
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


        public async Task<List<StudentLesson>> GetLessonStudentAsync(int lessonId)
        {
            throw new NotImplementedException();
        }


        public async Task KickStudentAsync(StudentLesson studentLesson)
        {
            var removedEntity = await _context.StudentLessons
                .SingleOrDefaultAsync(x=>x.LessonId==studentLesson.LessonId && x.StudentId==studentLesson.StudentId);
            _context.StudentLessons.Remove(removedEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<StudentLesson> SubscribeToLessonAsync(int studentId, int lessonId)
        {
            if (lessonId == 0 || studentId == 0)
            {
                return null;
            }
            StudentLesson studentLesson = new StudentLesson
            {
                LessonId = lessonId,
                StudentId = studentId
            };
            await _context.StudentLessons.AddAsync(studentLesson);
            await _context.SaveChangesAsync();
            return studentLesson;
        }

        public async Task<List<Lesson>> GetLessonsForStudentAsync(int studentId)
        {
            return await _context.StudentLessons
                .Where(sl => sl.StudentId == studentId)
                .Select(sl => sl.Lesson)
                .ToListAsync();
        }
    }
}
