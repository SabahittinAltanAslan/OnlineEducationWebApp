using OnlineEducationWebApp.Data.Entities;

namespace OnlineEducationWebApp.Interfaces
{
    public interface IStudentLessonService
    {
        public Task<List<StudentLesson>> GetLessonStudentAsync (int lessonId);

        public Task KickStudentAsync (StudentLesson studentLesson);

        public Task<StudentLesson>  SubscribeToLessonAsync (int studentId, int lessonId);
        public Task<StudentLesson> GetByIdForKickAsync(int lessonId, int studentId);
        public Task<List<Lesson>> GetLessonsForStudentAsync(int studentId);


    }
}
