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
        public async Task<List<StudentLesson>> GetLessonStudentAsync(int lessonId)
        {
            throw new NotImplementedException();
        }


        public async Task KickStudentAsync(int id)
        {
            var removedEntity = await _context.StudentLessons.FindAsync(id);
            _context.StudentLessons.Remove(removedEntity);
            await _context.SaveChangesAsync();
        }

        public async Task<StudentLesson> SubscribeToLessonAsync(StudentLesson studentLesson ,int studentId, int lessonId)
        {
            if(lessonId == 0 || studentId == 0)
            {
                return null;
            }
            studentLesson.StudentId = studentId;
            studentLesson.LessonId = lessonId;
            await _context.StudentLessons.AddAsync(studentLesson);
            await _context.SaveChangesAsync();
            return studentLesson;

        }
    }
}
