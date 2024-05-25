using OnlineEducationWebApp.Data.Entities;

namespace OnlineEducationWebApp.Interfaces
{
    public interface ILessonService
    {
        public Task<List<Lesson>> GetLessonsAsync();
        public Task<Lesson> GetLessonByIdAsync(int id);
        public Task<Lesson> CreateAsync(Lesson lesson);
        public Task UpdateAsync(Lesson lesson);
        public Task DeleteAsync(int id);

    }
}
