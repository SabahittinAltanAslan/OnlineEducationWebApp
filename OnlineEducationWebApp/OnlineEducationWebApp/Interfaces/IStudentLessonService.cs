using OnlineEducationWebApp.Data.Entities;

namespace OnlineEducationWebApp.Interfaces
{
    public interface IStudentLessonService
    {
        public Task<List<StudentLesson>> GetLessonStudentAsync (int lessonId);

        public Task KickStudentAsync (int id);

        public Task<StudentLesson>  SubscribeToLessonAsync (StudentLesson studentLesson, int studentId, int lessonId);

    }
}
