using OnlineEducationWebApp.Data.Entities;

namespace OnlineEducationWebApp.Interfaces
{
    public interface IStudentLessonService
    {
        public Task KickStudentAsync(StudentLesson studentLesson);

        public Task<StudentLesson> GetByIdForKickAsync(int lessonId, int studentId);
    }
}