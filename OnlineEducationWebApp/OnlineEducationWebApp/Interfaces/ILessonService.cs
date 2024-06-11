using OnlineEducationWebApp.Data.Entities;

namespace OnlineEducationWebApp.Interfaces
{
    public interface ILessonService
    {
        public Task<Lesson> GetLessonByIdAsync(int id);
        public Task<Lesson> CreateAsync(Lesson lesson, int id);
        public Task DeleteAsync(int id);

        public Task<List<Lesson>> GetTeacherLessonAsync(int id);

    }
}
