using OnlineEducationWebApp.Data.Entities;
using OnlineEducationWebApp.Models.DTO;

namespace OnlineEducationWebApp.Interfaces
{
    public interface ILessonService
    {
        Task<List<Lesson>> GetAllAsync();

        Task<Lesson> GetLessonByUrl(string url);

        Task<Lesson> GetLessonByIdAsync(int id);

        Task<Lesson> CreateAsync(LessonCreateRequest request, int teacherId);

        Task DeleteAsync(int id);

        Task<Lesson> JoinLesson(string lessonUrl, int userId);

        Task<Lesson> StartLesson(string lessonUrl);

        Task<Lesson> FinishLesson(int lessonId);

        Task<List<Lesson>> GetTeacherLessonAsync(int id);

        Task<List<Lesson>> GetStudentLessonAsync(int id);

        Task Subscribe(int studentId, int lessonId);

        Task<bool> IsStudentSubscribedToLesson(string lessonUrl, int studentId);
    }
}